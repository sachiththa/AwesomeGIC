Notes
-----
1. Solution is designed using clean architecture.
2. CQRS pattern is used to seperate reads and writes to database(arrays in this solution).
3. Dependency injection is implemented to make classes loosely coupled.
4. Static arrays are used for storing the transactions, accounts and interest rules in memory.

Assumptions
-----------
1. 3 transactions are already being inserted for Account : AC001
2. 2 interest rules are already created.
