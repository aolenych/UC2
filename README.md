The program contains 2 API endpoints - for displaying the balance and displaying the balance transactions.
	GetBalance() allow to see the current ballance on your Stripe account. 
	GetBalanceTransactions() shows any type of transactions which are received or debited from your account.
To use endpoints you should provide your Stripe secret key.

To run application locally, you should install .NET 6 SDK and Strip.net library. To use data from your Stripe account you should paste your secret key to appsettings.json or via secret manager.

Examples of URL requests:
https://localhost:7092/Balance/GetBalance()
https://localhost:7092/Balance/GetBalanceTransactions