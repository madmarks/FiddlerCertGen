﻿using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    internal class NCryptKeyOrCryptProviderSafeHandle : NCryptHandleBase
    {
        private bool _callerFree = true;
        public NCryptKeyOrCryptProviderSafeHandle(bool ownsHandle) : base(ownsHandle)
        {
        }

        public NCryptKeyOrCryptProviderSafeHandle() : base(true)
        {

        }

        public bool IsNCryptKey => PlatformSupport.HasCngSupport && NCrypt.NCryptIsKeyHandle(handle);

        protected override bool ReleaseHandle()
        {
            if (!_callerFree)
            {
                return true;
            }
            if (IsNCryptKey)
            {
                return NCrypt.NCryptFreeObject(handle) == SECURITY_STATUS.ERROR_SUCCESS;
            }
            else
            {
                return AdvApi32.CryptReleaseContext(handle, 0u);
            }
        }

        internal void SetCallerFree(bool callerFree)
        {
            _callerFree = callerFree;
        }
    }
}