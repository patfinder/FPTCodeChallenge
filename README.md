

## How to run this program

To run this program, follow below steps:

1. Open a CMD window (Open Run, enter "CMD", Enter)
2. Set environment variable by running below command:
	SET WEATHER_API_KEY=5466ed30a8ffcf523b01419670994ce0

	Above command set the API key for http://api.weatherstack.com

3. Open the solution and compile.
4. Go to compile output folder of FPTCodeChallenge project, which similar to below:
	FPTCodeChallenge\FPTCodeChallenge\bin\Debug\net8.0

5. Run the program FPTCodeChallenge.exe, enter Zipcode, Enter
6. The program will show the answers similar to below:


> Please enter your zip code: l5g  
========================================  
Should I go outside? Yes (Precip: 0)  
Should I wear sunscreen? No (UvIndex: 1)  
Can I fly my kite? No (WindSpeed: 6)  
========================================


7. To run program directly from Visual Studio, you can set WEATHER_API_KEY by going to
	FPTCodeChallenge project setting > Debug, click on "Open debug launch profile UI"
	In the opened window, scroll to "Environment Variables" and enter name/value for WEATHER_API_KEY.


## Code Explanation

- There are 2 main service in the project
  - WeatherService to retrieve weather data
  - AdviceService get weather information for specified location and return advices for 3 questions
		Should I go outside?
		Should I wear sunscreen?
		Can I fly my kite?

- The main program will configure the services in the form of Depedency Injection.
	DI will help in testing the program.

- The program also use config file (appsettings.json) which store settings for Weather API Url and Log settings.
- The API key is a kind of secret, so it is stored in environment (or keyvault if we use cloud) for security.

DTO folder: contains DTO objects to transfer Weather Service and Advice Service result.
Interface folder: contains the interfaces for Weather and Advice Service
Service folder: contains the implementation for Weather and Advice Service

Data\current.json
	This file store the Weather API output for reference.
