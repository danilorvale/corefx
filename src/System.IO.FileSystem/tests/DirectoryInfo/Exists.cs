﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using Xunit;

namespace System.IO.FileSystem.Tests
{
    public partial class DirectoryInfo_Exists : FileSystemTest
    {
        [Fact]
        public void TrueForCreatedDirectory()
        {
            DirectoryInfo di = Directory.CreateDirectory(GetTestFilePath());
            Assert.True(di.Exists);
        }

        [Fact]
        public void TrueForNewDirectoryInfo()
        {
            string dirName = GetTestFilePath();
            Directory.CreateDirectory(dirName);

            DirectoryInfo di = new DirectoryInfo(dirName);
            Assert.True(di.Exists);
        }

        [Fact]
        public void TrueForEnumeratedDir()
        {
            string dirName = GetTestFilePath();
            DirectoryInfo parent = Directory.CreateDirectory(dirName);
            parent.CreateSubdirectory("subDir");

            var dirs = parent.GetDirectories();
            Assert.Equal(1, dirs.Length);
            Assert.True(dirs[0].Exists);

            var fsis = parent.GetFileSystemInfos();
            Assert.Equal(1, fsis.Length);
            Assert.IsType<DirectoryInfo>(fsis[0]);
            Assert.True(fsis[0].Exists);

            var dirsEnum = parent.EnumerateDirectories();
            DirectoryInfo di = dirsEnum.FirstOrDefault();
            Assert.NotNull(di);
            Assert.True(di.Exists);


            var fsisEnum = parent.EnumerateFileSystemInfos();
            FileSystemInfo fsi = fsisEnum.FirstOrDefault();
            Assert.NotNull(fsi);
            Assert.IsType<DirectoryInfo>(fsi);
            Assert.True(fsi.Exists);
        }

        [Fact]
        public void ExistsBehaviorWithRefresh()
        {
            string dirName = GetTestFilePath();
            DirectoryInfo di = new DirectoryInfo(dirName);
            // don't check it, data has not yet been init'ed
            Directory.CreateDirectory(dirName);
            // data will be init'ed at the time of calling exists
            Assert.True(di.Exists);

            dirName = GetTestFilePath();
            di = new DirectoryInfo(dirName);

            Assert.False(di.Exists);
            Directory.CreateDirectory(dirName);

            // data should be stale
            Assert.False(di.Exists);

            // force refresh
            di.Refresh();
            Assert.True(di.Exists);
        }

        [Fact]
        public void FalseForFile()
        {
            string fileName = GetTestFilePath();
            File.Create(fileName);
            DirectoryInfo di = new DirectoryInfo(fileName);
            Assert.False(di.Exists);
        }
    }
}
