Welcome to Michelle Tours Derivco code test

Please find Two Steps, each in their own folder.


STEP 1:
	-- Install SP
	Open  file "pr_GetOrderSummary.sql" in SQL Server Management Studio 
	Connect to SQL instance containing NorthWind database
	Ensure "Northwind" database is the selected database
	Run the script (which installs the stored procedure "pr_GetOrderSummary")

	-- Run examples 
	Open File "Execute Examples.sql"in SQL Server Management Studio 
	Connect to SQL instance containing NorthWind database
	Ensure "Northwind" database is the selected database
	Run the script - and check the results (can be all results or line by line)

	--Notes / Assumptions	
	1. The specified Column "NumberOfOders" has a spelling mistake, I have corrected the output
	2. The specified Column "NumberOfDifferentProducts" is vague, I've assumed it to mean Unique products ordered (not count of order details, or sum of quantities)
	3. There is no rounding specified on the Total Order Value - results include long decimal places
	4. There is no order by specified - this could be useful but I've commented it out
	5. Assumed there is an instance of SQL server with a database named "NorthWind" installed -- Scripts have a USE Northwind Statement

STEP 2:

	Implemented Web Api wirth latest version of Visual Studio Community Edition.
	The project  uses .Net version 8.0 which came standard with VS2022
	I went with Entity Framework since I am most familiar with it - especially since it was my first time using SQL Lite.
	I have used the Repository patter to decouple the entities from the rest of the solution to make it unit testable.
	I implemented AutoMapper to map betwee Domain Entities and regular POCO's
	I have also implemented a combination of factory + stratregy patters (as an example to perform differen "spin" stratregies)

	As per the requirements:
	
	1. I have implemented async/await throughout the application and it should not be thread blocking.
	2. All requests return JSON body, and can be tested by running the application and executing the steps via the swagger interface
	3. I have implemented SOLID principles throughout, Including Single Responsibility Principle, Integration Segregation Principle and Dependency Inversion Principle. Although not directly intentended I believe I have also implemented Open-Closed Principle. There was no opportuinity to implement Liskov substitution Principle.
	4. I applied full TDD when implementing unit tests. I first decided on the idea, then wrote the failiing tests. Only after I have all tests written did I implement each on until it passed. 
	5. I implemented Global Exception Handlikng, See "ErrorHandlerMiddleware"
	6. The project is about 90% complete. All actions work except for the final payout. Unfortiunately work has caught up with my and I haven't been able to map the spin values to table locations which would determin the payout multiplier. 


To run the Roulette Game:

	1. Execute the "Begin Round" Method, this will start a new round (IsInplay = true) and return a RoundCorrelationId (GUID) which is used to correlate all further turns in the game
	2. Execute "Place Bets" Method, using the same RoundCorrelationId. You can pass in up to 4 bets up to a total of 5000
	3. Execute the "Spin"  Method - using the same RoundCorrelationId, this saves and returns the spin value
	4. Execute the Payout Mewthod - using the same RoundCorrelationId, this calculates all winnig bets as per the spin value and ends the round(IsInPlay = false)

You can return a single round by calling "GetPreviousRound" and passing in a roundCorrelationId
You can return all rounds by caling "GetPreviousRounds"




All in all it was a fun exercise. I've drawn on my experience in TDD and SOLID and provided a solution that is completely decpoupled, TDD and produces a simple API to run rounds of roulette. The SQL part of the exercise was also enjoyable I have no problem with that exercise.

Thank you for the opportunuty.

Michelle

