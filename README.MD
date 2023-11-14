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
	5. Assumed Whoever looks at this has an instance of SQL server with NorthWind installed

STEP 2:

	