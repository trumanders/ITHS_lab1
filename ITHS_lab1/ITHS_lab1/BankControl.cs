using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ITHS_lab1
{
    /**
     * This class handles all of the customers and has methods to extract
     * information from customer  object. It has methods to error check
     * the input and also get information about the accounts indirectly.
     * @author Anders Johansson, andjox-1
     */
    public class BankControl
    {
        private List<Customer> allCustomers = new List<Customer>();

        /**
         * Returns a string List with all customers' persNum and full name.
         */
        public List<string> getAllCustomers()
        {
            List<string> allCustomersPersNumAndName = new List<string>();
            foreach (Customer customer in allCustomers)
            {
                allCustomersPersNumAndName.Add(customer.getPersonalNumber() + " " + customer.getFullName());
            }
            return allCustomersPersNumAndName;
        }

        /**
         * Creates a new customer object. Checks if personal number is unique.
         * @param name      Customer's first name
         * @param surname   Customer's last name
         * @param pNo       Customer's personal number
         */
        public bool createCustomer(string name, string surname, string pNo)
        {
            // Check if personal number IS VALID (does not already exists)
            if (isPersonalNumberValid(pNo))
            {

                /* If personal number is valid, create the customer and add to the List */
                allCustomers.Add(new Customer(name, surname, pNo));
                return true;
            }
            return false;
        }


        /**
         * Retuns an List containing the customer's name, personal number, and it's accounts
         * and information about the customer's accounts.
         * @param pNo   Customer's personal number
         * @return      Info about the customer
         */
        public List<string> getCustomer(string pNo)
        {
            /* Search for the customer using the personal number pNo */
            int customerIndex = searchForCustomer(pNo);
            if (customerIndex < 0)
            {
                return null;
            }

            /* List to hold customer info to be returned */
            List<string> customerInfo = new List<string>();

            /* Add customer name as the first element of the customerInfo */
            customerInfo.Add(pNo + " " + allCustomers[customerIndex].getFullName());

            /* Then add the information about each account. */
            for (int i = 0; i < allCustomers[customerIndex].getNumberOfAccounts(); i++)
            {
                string accountNumber = Convert.ToString(allCustomers[customerIndex].getAccountNumber(i));
                string formatBalance = formatMoneyString(allCustomers[customerIndex].getAccountBalance(i));
                string formatInterestRate = formatPercentString(Account.getInterestRate());
                customerInfo.Add(accountNumber + " " + formatBalance + " " + Account.getACCOUNT_TYPE() + " " + formatInterestRate);
            }
            return customerInfo;
        }


        /**
         * Method to change a customer's name
         * @param name      The customer's new first name
         * @param surname   The customer's new last name
         * @param pNo       The pesonal number of the customer to change it's name
         * @return bool  false: if both strings passed are empty, or if the customer was not found
         *                  true: if the operation was successful
         */
        public bool changeCustomerName(string name, string surname, string pNo)
        {
            if (name.Length == 0 && surname.Length == 0)
            {
                return false;
            }
            /* Search for the customer to change name */
            int customerIndex = searchForCustomer(pNo);
            if (customerIndex < 0)
            {
                return false;
            }
            if (name.Length > 0) allCustomers[customerIndex].setfName(name);
            if (surname.Length > 0) allCustomers[customerIndex].setlName(surname);
            return true;
        }


        /**
         * Create a new account for a specified customer
         * @param pNo   The personal number of the customer to create a new account for
         * @return int  The account number of the created account
         */
        public int createSavingsAccount(string pNo)
        {
            int customerIndex = searchForCustomer(pNo);
            if (customerIndex < 0)
            {
                return -1;
            }
            allCustomers[customerIndex].addAccount();
            return Account.getCountingAllAccountNumbers();
        }


        /**
         * Gets information about a specific account: account number, balance, account type, interest rate
         * @param pNo           The personal number of the customer in possession of the account
         * @param accountId     The accont number for the account to get information about
         * @return string       A string containing the information, return null if the customer or acccount was not found
         */
        public string getAccount(string pNo, int accountId)
        {
            int customerIndex = searchForCustomerAndAccount(pNo, accountId)[0];
            int accountIndex = searchForCustomerAndAccount(pNo, accountId)[1];

            /* If customer or account doesn't exist */
            if (customerIndex < 0 || accountIndex < 0)
            {
                return null;
            }

            /* Get info */
            string accountNumber = Convert.ToString(allCustomers[customerIndex].getAccountNumber(accountIndex));
            string balance = formatMoneyString(allCustomers[customerIndex].getAccountBalance(accountIndex));
            string accountType = Account.getACCOUNT_TYPE();
            string interestRate = formatPercentString(Account.getInterestRate());
            return accountNumber + " " + balance + " " + accountType + " " + interestRate;
        }


        /**
         * Performs a depoit to the specified account
         * @param pNo       The personal number of the customer
         * @param accountId The account number to make a deposit to
         * @param amount    The amount of money to transfer to the account
         * @return bool  Returns true of the amount was > 0, both customer and it's account was found, and the deposit was made.
         */
        public bool deposit(string pNo, int accountId, int amount)
        {
            if (amount < 0)
                return false;

            /* Look for customer and account */
            int customerIndex = searchForCustomerAndAccount(pNo, accountId)[0];
            int accountIndex = searchForCustomerAndAccount(pNo, accountId)[1];
            if (customerIndex < 0 || accountIndex < 0)
                return false;

            /* Make deposit */
            allCustomers[customerIndex].makeDeposit(amount, accountIndex);
            return true;
        }


        /**
         * Makes a withdrawal from an account
         * @param pNo       The customer's personal number
         * @param accountId The account number to make the withdrawal from
         * @param amount    The amount of money to withdraw
         * @return bool  Returns true of the amount was > 0, both customer and it's account was found, and the withdrawal was made.
         */
        public bool withdraw(string pNo, int accountId, int amount)
        {
            int customerIndex = searchForCustomerAndAccount(pNo, accountId)[0];
            int accountIndex = searchForCustomerAndAccount(pNo, accountId)[1];

            /* If amount is less than 0, or if customer or account doesn't exist */
            if (amount < 0 || (customerIndex < 0 || accountIndex < 0))
                return false;

            /* Check if there is enough money on the account */
            if (allCustomers[customerIndex].getAccountBalance(accountIndex).CompareTo(new Decimal(amount)) >= 0)
            {
                allCustomers[customerIndex].makeWithdrawal(amount, accountIndex);
                return true;
            }
            return false;
        }


        /**
         * Closes an account and returns information abount the closed account
         * @param pNo       The personal number of the customer
         * @param accountId The account number for the account to be closed
         * @return string   Returns Accont number, balance, account tyoe, and interest before it is closed
         */
        public string closeAccount(string pNo, int accountId)
        {
            int customerIndex = searchForCustomerAndAccount(pNo, accountId)[0];
            int accountIndex = searchForCustomerAndAccount(pNo, accountId)[1];

            /* If customer or account does not exist */
            if (customerIndex < 0 || accountIndex < 0)
                return null;

            /* Get account info */
            string deletedAccountInfo = getDeletedAccountInfo(customerIndex, accountIndex);

            /* Delete the account */
            allCustomers[customerIndex].deleteAccount(accountIndex);
            return deletedAccountInfo;
        }


        /**
         * Removes a customer and it's accounts
         * @param pNo           The customers personal number
         * @return List    A list of information about the deleted customer and it's accounts
         */
        public List<string> deleteCustomer(string pNo)
        {
            /* Get the index of the customer to delete */
            int customerIndex = searchForCustomer(pNo);

            /* If personal number not found, return null, else start collecting info */
            if (customerIndex < 0)
                return null;
            else
            {
                List<string> deletedCustomerInfo = new List<string>();

                /* Add personal number and name as the first element */
                deletedCustomerInfo.Add(pNo + " " + allCustomers[customerIndex].getFullName());

                /* Add information about each account in the following elements of the List */
                for (int i = 0; i < allCustomers[customerIndex].getNumberOfAccounts(); i++)
                    deletedCustomerInfo.Add(getDeletedAccountInfo(customerIndex, i));

                /* Delete customer accounts */
                for (int i = 0; i < allCustomers[customerIndex].getNumberOfAccounts(); i++)
                    allCustomers[customerIndex].deleteAccount(i);

                /* Delete customer */
                allCustomers.RemoveAt(customerIndex);
                return deletedCustomerInfo;
            }
        }



        // CUSTOM METHODS //

        /**
         * Checks if the personal number of a new customer is valid
         * @param pNo       The personal number to be validated
         * @return bool  Returns true if the personal number was valid (didn't alreay exist), else returns false
         */
        private bool isPersonalNumberValid(string pNo)
        {
            bool isValid = true;
            foreach (Customer customer in allCustomers)
            {
                if (customer.getPersonalNumber() == pNo)
                {
                    return false;
                }
            }
            return true;
        }


        /**
         * Looks for a customer
         * @param pNo       The personal number to search for
         * @return int      The index of the customer in the customer List. Returns -1 if the customer was not found
         */
        private int searchForCustomer(string pNo)
        {
            int customerIndex = -1;
            foreach (Customer customer in allCustomers)
            {
                customerIndex++;
                if (customer.getPersonalNumber() == pNo)
                {
                    return customerIndex;
                }
            }
            return -1;
        }


        /**
         * Checks if a customer and a specified account exists
         * @param pNo           The personal number of the customer to look for
         * @param accountNumber The account number to look for
         * @return int[2]        The index of the customer, and the index of the account
         */
        private int[] searchForCustomerAndAccount(string pNo, int accountNumber)
        {
            /* Holds the index of the customer, and the index of the account */
            int[] customerIndexAccountIndex = new int[2];

            /* Search and set the index */
            customerIndexAccountIndex[0] = searchForCustomer(pNo);

            /* If the customer was not found, return */
            if (customerIndexAccountIndex[0] < 0)
            {
                return customerIndexAccountIndex;
            }
            customerIndexAccountIndex[1] = allCustomers[customerIndexAccountIndex[0]].searchForCustomerAccount(accountNumber);
            return customerIndexAccountIndex;
        }


        /**
         * Formats a Decimal number as swedish money
         * @param money     The Decimal number to be formatted
         * @return string   The Decimal as a string
         */
        private string formatMoneyString(Decimal money)
        {
            NumberFormatInfo decimals = new NumberFormatInfo();
            decimals.NumberDecimalDigits = 2;
            return money.ToString("N", decimals) + " kr";
        }


        /* Format the output of the interest rate */
        private string formatPercentString(Decimal interestRate)
        {
            NumberFormatInfo decimals = new NumberFormatInfo();
            return interestRate.ToString("N", decimals) + "%";
        }


        /**
         * Gets information about an account to be deleted
         * @param customerIndex     The index of the customer object to get the information from
         * @param accountIndex      The index of the account to get the information from
         * @return string           Account number, balance, account type, and interest
         */
        private string getDeletedAccountInfo(int customerIndex, int accountIndex)
        {
            string balance = formatMoneyString(allCustomers[customerIndex].getAccountBalance(accountIndex));
            string interest = formatMoneyString(allCustomers[customerIndex].calculateInterest(accountIndex));
            return allCustomers[customerIndex].getAccountNumber(accountIndex) + " " + balance + " " + Account.getACCOUNT_TYPE() + " " + interest;
        }
    }
}
