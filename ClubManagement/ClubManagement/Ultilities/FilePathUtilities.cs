using System;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Provider;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;

namespace ClubManagement.Ultilities
{
    public static class FilePathUtilities
    {
        public static string GetAbsoluteFilePath(Context context, Uri fileUri)
        {
            string realPath;

            if (Build.VERSION.SdkInt < BuildVersionCodes.Base11)
            {
                realPath = GetRealPathFromURI_BelowAPI11(context, fileUri);
            }

            else if (Build.VERSION.SdkInt < BuildVersionCodes.Kitkat)
            {
                realPath = GetRealPathFromURI_API11to18(context, fileUri);
            }

            else
            {
                realPath = GetRealPathFromURI_API19(context, fileUri);
            }
            return realPath;
        }

        private static string GetRealPathFromURI_API11to18(Context context, Uri contentUri)
        {
            var proj = new[] {MediaStore.Images.Media.InterfaceConsts.Data};

            var cursorLoader = new CursorLoader(context, contentUri, proj, null, null, null);

            if (!(cursorLoader.LoadInBackground() is ICursor cursor)) return null;
            var columnIndex = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
            cursor.MoveToFirst();
            var result = cursor.GetString(columnIndex);
            cursor.Close();
            return result;
        }

        private static string GetRealPathFromURI_BelowAPI11(Context context, Uri contentUri)
        {
            string[] proj = {MediaStore.Images.Media.InterfaceConsts.Data};
            var cursor = context.ContentResolver.Query(contentUri, proj, null, null, null);
            var result = "";
            if (cursor == null) return result;
            var columnIndex = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
            cursor.MoveToFirst();
            result = cursor.GetString(columnIndex);
            cursor.Close();
            return result;
        }

        private static string GetRealPathFromURI_API19(Context context, Uri uri)
        {

            var isKitKat = Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat;

            if (isKitKat && DocumentsContract.IsDocumentUri(context, uri))
            {
                if (IsExternalStorageDocument(uri))
                {
                    var docId = DocumentsContract.GetDocumentId(uri);
                    var split = docId.Split(':');
                    var type = split[0];

                    if (string.Equals(type, "primary", StringComparison.OrdinalIgnoreCase))
                    {
                        return Environment.ExternalStorageDirectory + "/" + split[1];
                    }
                }
                else if (IsDownloadsDocument(uri))
                {

                    var id = DocumentsContract.GetDocumentId(uri);
                    var contentUri = ContentUris.WithAppendedId(
                        Uri.Parse("content://downloads/public_downloads"), long.Parse(id));

                    return GetDataColumn(context, contentUri, null, null);
                }
                else if (IsMediaDocument(uri))
                {
                    var docId = DocumentsContract.GetDocumentId(uri);
                    var split = docId.Split(':');
                    var type = split[0];

                    Uri contentUri = null;
                    switch (type)
                    {
                        case "image":
                            contentUri = MediaStore.Images.Media.ExternalContentUri;
                            break;
                        case "video":
                            contentUri = MediaStore.Video.Media.ExternalContentUri;
                            break;
                        case "audio":
                            contentUri = MediaStore.Audio.Media.ExternalContentUri;
                            break;
                    }

                    const string selection = "_id=?";
                    var selectionArgs = new[]
                    {
                        split[1]
                    };

                    return GetDataColumn(context, contentUri, selection, selectionArgs);
                }
            }
            else if (string.Equals("content", uri.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return IsGooglePhotoUri(uri) ? uri.LastPathSegment : GetDataColumn(context, uri, null, null);
            }
            else if (string.Equals("file", uri.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return uri.Path;
            }

            return null;
        }

        private static string GetDataColumn(Context context, Uri uri, string selection,
            string[] selectionArgs)
        {

            ICursor cursor = null;
            const string column = "_data";
            var projection = new[]
            {
                column
            };

            try
            {
                cursor = context.ContentResolver.Query(uri, projection, selection, selectionArgs,
                    null);
                if (cursor != null && cursor.MoveToFirst())
                {
                    var index = cursor.GetColumnIndexOrThrow(column);
                    return cursor.GetString(index);
                }
            }
            finally
            {
                cursor?.Close();
            }

            return null;
        }

        private static bool IsGooglePhotoUri(Uri uri)
        {
            return "com.google.android.apps.photos.content".Equals(uri.Authority);
        }

        private static bool IsExternalStorageDocument(Uri uri)
        {
            return "com.android.externalstorage.documents".Equals(uri.Authority);
        }

        private static bool IsDownloadsDocument(Uri uri)
        {
            return "com.android.providers.downloads.documents".Equals(uri.Authority);
        }

        public static bool IsMediaDocument(Uri uri)
        {
            return "com.android.providers.media.documents".Equals(uri.Authority);
        }
    }
}