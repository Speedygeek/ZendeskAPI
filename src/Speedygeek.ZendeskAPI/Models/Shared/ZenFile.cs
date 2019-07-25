// Copyright (c) Elizabeth Schneider. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.IO;

namespace Speedygeek.ZendeskAPI.Models
{
    /// <summary>
    /// A file to be attached to a <see cref="ZenEntity"/>
    /// </summary>
    public class ZenFile : IDisposable
    {
        private bool _disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Finalizes an instance of the <see cref="ZenFile"/> class.
        /// </summary>
        ~ZenFile()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        /// <summary>
        /// File Name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File Content Type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// File Data as a <see cref="Stream"/>
        /// </summary>
        public Stream FileData { get; set; }

        /// <summary>
        /// clean up memory
        /// </summary>
        /// <param name="disposing">should manged memory be clean up</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    FileData?.Dispose();
                }

                _disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
