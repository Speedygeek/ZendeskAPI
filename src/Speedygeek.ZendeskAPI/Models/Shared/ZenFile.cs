using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Speedygeek.ZendeskAPI.Models
{
    /// <summary>
    /// A file to be attached to a <see cref="ZenEntity"/>
    /// </summary>
    public class ZenFile : IDisposable
    {
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

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

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

         ~ZenFile()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
