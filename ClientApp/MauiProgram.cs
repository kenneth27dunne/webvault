using Microsoft.AspNetCore.Components.WebView.Maui;
using ClientApp.Data;
using Microsoft.AspNetCore.Components.Authorization;
using ClientApp.Services;
using ClientApp.Services.Interfaces;
using ClientApp.Extensions;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using ClientApp.Models;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace ClientApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
	#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
	#endif
		        
        builder.Services.AddAuthorizationCore(); 
		builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

		FirebaseApp.Create(new AppOptions()
		{
			Credential = GoogleCredential.FromStream(FileSystem.OpenAppPackageFileAsync("baseauthapp-9b279-firebase-adminsdk-qfcmn-6c2cf783d2.json").Result)
		});

        var localPath = Path.Combine(FileSystem.CacheDirectory, "baseauthapp-9b279-firebase-adminsdk-qfcmn-6c2cf783d2.json");

        using var json = FileSystem.OpenAppPackageFileAsync("baseauthapp-9b279-firebase-adminsdk-qfcmn-6c2cf783d2.json").Result;
        using var dest = File.Create(localPath);
        json.CopyToAsync(dest).Wait();

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", localPath);

		builder.Services.AddFirestore(Constants.FirebaseProjectId);
		builder.Services.AddFirebaseAuth(Constants.API_KEY); 
		builder.Services.AddScoped<CustomAuthenticationStateProvider>();                                                                                    
        builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthenticationStateProvider>());

        builder.Services.AddTransient<IFirestoreManager, FirestoreManager>();
        builder.Services.AddTransient<IAuthManager, AuthManager>();

        builder.Services.AddSingleton<WeatherForecastService>();


        return builder.Build();
	}
}
