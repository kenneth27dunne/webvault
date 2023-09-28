using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using ClientApp.Services.Interfaces;

namespace com.mycompany.WebVault;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (Intent.ActionSend.Equals(Intent.Action))
        {
            var stream = (Android.Net.Uri)Intent.GetParcelableExtra(Intent.ExtraStream);
            if (stream != null)
            {
                // Get the file path from the stream
                string filePath = GetFilePathFromStream(stream);

                // Call the Dependency Service to handle the received file
                DependencyService.Get<IFileHandler>().HandleReceivedFile(filePath);
            }
        }
    }

    private string GetFilePathFromStream(Android.Net.Uri uri)
    {
        string filePath = null;
        if (ContentResolver.SchemeContent.Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase))
        {
            // The Uri is a content Uri
            using var cursor = ContentResolver.Query(uri, null, null, null, null);
            if (cursor != null && cursor.MoveToFirst())
            {
                int columnIndex = cursor.GetColumnIndex(MediaStore.MediaColumns.Data);
                if (columnIndex != -1)
                {
                    filePath = cursor.GetString(columnIndex);
                }
            }
        }
        else if (ContentResolver.SchemeFile.Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase))
        {
            // The Uri is a file Uri
            filePath = uri.Path;
        }

        return filePath;
    }

}
