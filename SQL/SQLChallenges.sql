-- Parking Lot*******
-- *                *
-- *                *
--- *****************



-- Comment can be done single line with --
-- Comment can be done multi line with /* */

/*
DQL - Data Query Language
Keywords:

SELECT - retrieve data, select the columns from the resulting set
FROM - the table(s) to retrieve data from
WHERE - a conditional filter of the data
GROUP BY - group the data based on one or more columns
HAVING - a conditional filter of the grouped data
ORDER BY - sort the data
*/


-- BASIC CHALLENGES
-- List all customers (full name, customer id, and country) who are not in the USA

SELECT FirstName, LastName, CustomerId, Country
FROM Customer
WHERE Country != 'USA';
-- List all customers from Brazil 

SELECT * FROM Customer
WHERE Country = 'Brazil';


-- List all sales agents

SELECT * FROM Employee
WHERE Title LIKE '%Sales%';

-- SELECT * FROM employee WHERE title LIKE '%Agent%;


-- Retrieve a list of all countries in billing addresses on invoices
SELECT BillingAddress, BillingCountry FROM Invoice;

-- Retrieve how many invoices there were in 2009, and what was the sales total for that year?

SELECT InvoiceDate, Total FROM Invoice
WHERE InvoiceDate = 2009;

-- (challenge: find the invoice count sales total for every year using one query)


-- how many line items were there for invoice #37
SELECT COUNT(*) FROM InvoiceLine
WHERE InvoiceId = 37;


-- how many invoices per country? BillingCountry  # of invoices 

SELECT BillingCountry, COUNT(*) 
FROM Invoice
GROUP BY BillingCountry;

-- Retrieve the total sales per country, ordered by the highest total sales first.

SELECT BillingCountry, SUM(Total)
FROM Invoice
GROUP BY BillingCountry
ORDER BY BillingCountry DESC;

-- JOINS CHALLENGES
-- Every Album by Artist

SELECT a.Title, b.Name
FROM dbo.Album AS a 
JOIN dbo.Artist AS b ON a.ArtistId = b.ArtistId

-- (inner keyword is optional for inner join)

-- All songs of the rock genre

SELECT * FROM Genre
WHERE Name = 'Rock';

-- Show all invoices of customers from brazil (mailing address not billing)

SELECT a.Address, b.InvoiceId 
FROM dbo.Customer AS a
JOIN dbo.Invoice AS b ON a.CustomerId = b.CustomerId
WHERE a.country = 'Brazil';

-- Show all invoices together with the name of the sales agent for each one
SELECT a.InvoiceId,  c.FirstName, c.LastName 
FROM dbo.Invoice AS a
JOIN dbo.Customer AS b ON a.CustomerId = b.CustomerId 
JOIN dbo.Employee AS c ON b.SupportRepId = c.EmployeeId

SELECT * FROM Invoice
SELECT * FROM Employee
SELECT * FROM Customer

-- Which sales agent made the most sales in 2009?


-- How many customers are assigned to each sales agent?

SELECT COUNT(*) AS customers, b.FirstName, b.LastName
FROM dbo.Employee AS b
JOIN dbo.Customer AS a ON a.SupportRepId = b.EmployeeId
GROUP BY b.FirstName, b.LastName;

SELECT * FROM Customer
SELECT * FROM Employee; 

-- Which track was purchased the most in 2021?

SELECT TOP 1 t.Name, COUNT(*) AS Total
FROM dbo.Invoice AS i 
JOIN dbo.InvoiceLine AS il ON i.InvoiceId = il.InvoiceId
JOIN dbo.Track AS t ON t.TrackId = il.TrackId
WHERE YEAR(i.InvoiceDate) = 2021
GROUP BY t.Name
ORDER BY COUNT(*) DESC;

SELECT * FROM Track;
SELECT * FROM Invoice;
SELECT * FROM InvoiceLine;


-- Show the top three best selling artists.

SELECT TOP 3 a.Name, COUNT(*) AS BestSeller
FROM dbo.InvoiceLine AS il 
JOIN dbo.Track AS t ON t.TrackId = il.TrackId
JOIN dbo.Album AS al ON al.AlbumId = t.AlbumId
JOIN dbo.Artist AS a ON a.ArtistId = al.ArtistId
GROUP BY a.Name
ORDER BY COUNT(*) DESC;

SELECT * FROM InvoiceLine
SELECT * FROM Track
SELECT * FROM Artist
SELECT * FROM Album



-- Which customers have the same initials as at least one other customer?




-- Which countries have the most invoices?


-- Which city has the customer with the highest sales total?


-- Who is the highest spending customer?


-- Return the email and full name of of all customers who listen to Rock.


-- Which artist has written the most Rock songs?


-- Which artist has generated the most revenue?




-- ADVANCED CHALLENGES
-- solve these with a mixture of joins, subqueries, CTE, and set operators.
-- solve at least one of them in two different ways, and see if the execution
-- plan for them is the same, or different.

-- 1. which artists did not make any albums at all?


-- 2. which artists did not record any tracks of the Latin genre?


-- 3. which video track has the longest length? (use media type table)



-- 4. boss employee (the one who reports to nobody)


-- 5. how many audio tracks were bought by German customers, and what was
--    the total price paid for them?



-- 6. list the names and countries of the customers supported by an employee
--    who was hired younger than 35.




-- DML exercises

-- 1. insert two new records into the employee table.

-- 2. insert two new records into the tracks table.

-- 3. update customer Aaron Mitchell's name to Robert Walter

-- 4. delete one of the employees you inserted.

-- 5. delete customer Robert Walter.
