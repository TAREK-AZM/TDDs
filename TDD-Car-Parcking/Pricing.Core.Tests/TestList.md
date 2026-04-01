
- [x]  A pricing table must have a least one tier not empty
- [x]  A pricing table should contains set of Hours
- [x]  A pricing table cover 24 hours
- [x]  A Pricing table ordered desc by hour
- [x]  max price  <= 24 price on table if there is MaxDaily
- [x]  max price eq to price of 24 on the table

-------
- [x]   improve exception based tests
- [x]   hours limit should be between 1 and 24
- [x]   price can't be negative


-------- Application Service

- [x] Fail request not null 
- [x] should return true of success
- [x] invoke storage only once
- [x]  request is mapped correctly to storage
- [] External dependency is invoked to store pricing and that it stored the input
- 
--------

- [x] Throw exception if missing connection 
- [x] Insert if not exists
- [x] Replace if exists
- [x] Return true id succeed


--------

- [] 500 if unknown error
- [] 200 if success
- [] 400 if returns false

-------

- [] entry and exit time
- [] Get pricing table from storage
- [] should include the partial price
- [] calculate the total factore
