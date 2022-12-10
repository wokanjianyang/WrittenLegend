using System;
using SA.Android.Manifest;
using SA.iOS.Photos;

namespace SA.CrossPlatform.App
{
    class UM_PhotosPermission : UM_Permission
    {
        protected override UM_AuthorizationStatus IOSAuthorization
        {
            get
            {
                var status = ISN_PHPhotoLibrary.AuthorizationStatus;
                switch (status)
                {
                    case ISN_PHAuthorizationStatus.Authorized:
                        return UM_AuthorizationStatus.Granted;
                    default:
                        return UM_AuthorizationStatus.Denied;
                }
            }
        }

        protected override void IOSRequestAccess(Action<UM_AuthorizationStatus> callback)
        {
            ISN_PHPhotoLibrary.RequestAuthorization((status) =>
            {
                if (status == ISN_PHAuthorizationStatus.Authorized)
                    callback.Invoke(UM_AuthorizationStatus.Granted);
                else
                    callback.Invoke(UM_AuthorizationStatus.Denied);
            });
        }

        protected override AMM_ManifestPermission[] AndroidPermissions
        {
            get { return new[] { AMM_ManifestPermission.WRITE_EXTERNAL_STORAGE, AMM_ManifestPermission.READ_EXTERNAL_STORAGE }; }
        }
    }
}
