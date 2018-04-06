using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using NETMP.FileSystemListener.Common;
using NETMP.FileSystemListener.Common.Configuration;
using Messages = NETMP.FileSystemListener.Common.Resources.Messages;
using Dates = NETMP.FileSystemListener.Common.Resources.Dates;

namespace NETMP.FileSystemListener
{
    public class FileSystemListener
    {
        private readonly FileSystemListenerConfig _config;
        private readonly ILogger _logger;

        private readonly List<FileSystemWatcher> _fileSystemWatchers;
        private readonly string _defaultFolderPath;

        private static int _fileIndexNumber;

        public FileSystemListener(FileSystemListenerConfig config, ILogger logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _fileSystemWatchers = new List<FileSystemWatcher>();
            _fileIndexNumber = 1;
            _defaultFolderPath = _config.Rules.SingleOrDefault(rule => rule.FilePathTemplate == "default")?.MovingDestinationFolder;
        }

        public void StartWatching()
        {
            foreach (var folder in _config.ObservableFolders)
            {
                var watcher = new FileSystemWatcher(folder.Path);
                watcher.EnableRaisingEvents = true;
                watcher.Created += OnFileAdded;

                _fileSystemWatchers.Add(watcher);
            }

            _logger.LogInfo(string.Format(Messages.StartWatchingFolders));
        }

        #region Private methods

        private void OnFileAdded(object sender, FileSystemEventArgs e)
        {
            SetCultureInfo();

            _logger.LogInfo(string.Format(Messages.NewFileAdded, Path.GetDirectoryName(e.FullPath)));

            var rule = GetMatchingRule(e);

            var destinationFileName = UpdateFileName(rule, e.Name);
            var destinationPath = GetDestinationFolderPath(rule, e, destinationFileName);

            _logger.LogInfo(string.Format(Messages.FileIsMoving, e.Name, destinationPath));

            File.Move(e.FullPath, destinationPath);

            _logger.LogInfo(string.Format(Messages.FileMoved, e.Name, destinationPath));
        }

        private void SetCultureInfo()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(_config.Culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(_config.Culture);
        }

        private RuleElement GetMatchingRule(FileSystemEventArgs e)
        {
            return _config.Rules.SingleOrDefault(rule => rule.FilePathTemplate != "default" && 
                                                         new Regex(rule.FilePathTemplate).IsMatch(e.Name));
        }

        private string UpdateFileName(RuleElement rule, string fileName)
        {
            if (rule != null)
            {
                var fileExtension = Path.GetExtension(fileName);

                fileName = fileName.Replace(fileExtension, string.Empty);

                if (rule.AddIndexNumber) fileName += _fileIndexNumber++.ToString();

                if (rule.AddMovingDate) fileName += DateTime.Today.ToString(Dates.Date);

                fileName += fileExtension;
            }

            return fileName;
        }

        private string GetDestinationFolderPath(RuleElement rule, FileSystemEventArgs e, string outFileName)
        {
            var destinationFolderPath = rule != null ? rule.MovingDestinationFolder : _defaultFolderPath;
            var logInfoMessage = rule != null
                                 ? string.Format(Messages.RulePassed, e.Name, rule.FilePathTemplate, rule.MovingDestinationFolder)
                                 : string.Format(Messages.RuleNotPassed, e.Name, _defaultFolderPath);

            _logger.LogInfo(logInfoMessage);

            return Path.Combine(destinationFolderPath, outFileName);
        }

        #endregion
    }
}
