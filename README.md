    	   Inventory and Shipment Management System 

Overview
	Inventory And Shipment Management System API  
This is developed Using the .NET Core API to perform the backend activity for the CRUD implementation of Inventory and Shipment Management System.
	Inventory And Shipment Management System UI  
This is developed using Windows Forms and utilizes an API to perform CRUD (Create, Read, Update, Delete) operations. The application enables users to create the product inventory and then do the shipment assign for the respective product to manage inventory items efficiently by providing a user-friendly interface and robust backend functionality.

Features
•	Add, Update, Delete, View Shipments and Export the product to CSV for the inventory (Product) items.
•	Dependency Injection (Constructor Injection).
•	Logging implementation to trace the logs.
•	Unit test cases for code coverage (N-Unit Framework).
•	User-friendly interface with Windows Forms.

Technical Stack
•	Frontend - .NET 8 Windows Form, C#, WEB API (REST API) Integration.
•	Backend - .NET Core (.NET 8), C#, SQL Server, ADO.NET, Entity Framework

Setup Instructions
	Prerequisites
•	Visual Studio 2022
•	Windows Forms: NET 8.0-windows
•	API: NET 8.0 for Rest API
•	SQL Server 2019 or any compatible database


Installation
	Clone the repository: 
git clone API -> https://github.com/Sanjaynigam91/InventoryAndShipmentManagementSystemAPI 
git clone UI->  https://github.com/Sanjaynigam91/InventoryAndShipmentManagementSystemUI
	Open the Solution in Admin Mode: 
o	API -> Open InventoryAndShipmentManagementSystem.sln in Visual Studio.
o	UI -> Open InventoryManagementSystemUI.sln in Visual Studio.
	Configure the Database: 
o	Update the connection string in appsettings.json to point to your database connection string(InventoryDbConnection).
	Run Migrations: 
o	Open the Package Manager Console in Visual Studio and run:
	Build the solution and run the application from Visual Studio.
Usage
1.	Add Inventory Items: 
o	Use the form to input product details for new inventory items and add them to the database.
2.	Inventory List 
o	Display the inventory details for the product from this page.
o	Product details can be edit and delete from this page.
o	Export the inventory details into csv files by click on the export to csv.
o	View the product’s shipment history details by click on the view shipments.
o	Select the product then click on the shipment option to go to the assign shipment page to assign the product for shipment. 
3.	Edit and Delete Items: 
o	Select an item from the list and edit its details from add/edit form. 
o	Select the product from the list and then perform the delete operation.
4.	Assign Shipment: -
    This screen is used to assign the product inventory to shipments (Ground, Air, Sea).
5.	Shipment History
	Display the shipment history details for the all-product inventory those are assigned for shipment.
	Export the shipment history into csv from this page.

6.	Logging 
o	All operations are logged in Logs folder in separate text file based on dates to handle exception & debugging purposes.





