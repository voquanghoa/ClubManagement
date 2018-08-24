using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Provider;

namespace ClubManagement.Controllers
{
    public static class CloudinaryController
    {
        private const string CloudName = "dw0yzvsvn";

        private const string ApiKey = "759427256828639";

        private const string ApiSecret = "ventEbxZBzh3g5EAfVw7htDIbDA";

        public static async Task<string> UploadImage(Context context, Android.Net.Uri imageUri, string publicId)
        {
            var account =
                new CloudinaryDotNet.Account(CloudName, ApiKey, ApiSecret);

            var cloudinary = new CloudinaryDotNet.Cloudinary(account);

            var uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams
            {
                File = new CloudinaryDotNet.Actions.FileDescription(GetFilePathFromUri(context, imageUri)),
                PublicId = publicId
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            return uploadResult.Uri.OriginalString;
        }

        private static string GetFilePathFromUri(Context context, Android.Net.Uri imageUri)
        {
            try
            {
                using (var cursor = context.ContentResolver.Query(imageUri,
                    new[] { MediaStore.Images.Media.InterfaceConsts.Data }, null, null, null))
                {
                    if (cursor == null) return string.Empty;
                    var columnIndex =
                        cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
                    cursor.MoveToFirst();
                    return cursor.GetString(columnIndex);
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}