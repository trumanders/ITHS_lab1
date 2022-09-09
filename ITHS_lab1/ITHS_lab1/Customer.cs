using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHS_lab1
{
    /**
     * This class defines the customers. It has methods to make deposit, withdrawal, calculate
     * interest, and search for a customer's accounts, and hand out information about a customer's
     * accounts.
     * @author Anders Johansson, anders.johansson@iths.se
     */

    public class Customer
    {
        private string fName;
        private string lName;
        private string personalNumber;

    /* Contains a list of the customer's accounts */
    private List<Account> thisCustomersAccounts = new List<Account>();

        public Customer(string fName, string lName, string persNum)
        {
            this.fName = fName;
            this.lName = lName;
            this.personalNumber = persNum;
        }

        /* SETTERS */

        /**
         * Sets the customer's first name
         * @param fName     The customer's first name
         */
        public void setfName(string fName)
        {
            this.fName = fName;
        }

        /**
         * Sets the customer's last name
         * @param lName     The customer's last name
         */
        public void setlName(string lName)
        {
            this.lName = lName;
        }

        /**
         * Creates a new account in the account-List
         */
        public void addAccount()
        {
            this.thisCustomersAccounts.Add(new Account());
        }


        /**
         * Deletes the account with the specified index in the account-List
         * @param index     The index of the account to be deleted
         */
        public void deleteAccount(int index)
        {
            thisCustomersAccounts.Remove(index);
        }


        /**
         * The method calls the deposit method for the specified account
         * @param amount    The amount to pass to the deposit method
         * @param index     The index of the account
         */
        public void makeDeposit(int amount, int index)
        {
            thisCustomersAccounts[index].deposit(amount);
        }


        /**
         * Method to call the withdraw-method for the specified account
         * @param amount    The amount to pass to the withdraw method
         * @param index     The index of the account
         */
        public void makeWithdrawal(int amount, int index)
        {
            thisCustomersAccounts[index].withdraw(amount);
        }


        // GETTERS //

        /**
         * Get the customer's personal number
         * @return string   The customer's personal number
         */
        public string getPersonalNumber()
        {
            return this.personalNumber;
        }

        /**
         * Gets the customer's first name
         * @return string   The customer's first name
         */
        public string getfName()
        {
            return this.fName;
        }

        /**
         * Gets the customer's last name
         * @return string   The customer's last name
         */
        public string getlName()
        {
            return this.lName;
        }

        /**
         * Gets the customers first and last name.
         * @return string   The customer's first and last name separated by space.
         */
        public string getFullName()
        {
            return getfName() + " " + getlName();
        }


        /**
         * Gets the number of accounts of a customer
         * @return int  The number of accounts that the customer has
         */
        public int getNumberOfAccounts()
        {
            return thisCustomersAccounts.Count;
        }


        /**
         * Gets the account numbers of a customer's accounts
         * @return ArrrayList<Integer>  List of all the account numbers
         */
        public int getAccountNumber(int index)
        {
            return thisCustomersAccounts[index].getAccountNumber();
        }


        /**
         * Gets the balance of all accounts of the customer
         * @return List<Decimal>    List of the balances of the accounts
         */
        public Decimal getAccountBalance(int index)
        {
            return thisCustomersAccounts[index].getBalance();
        }


        /**
         * Checks if an account exists and gets it's index in the account List
         * @param accountNum    The account number to look up
         * @return int          The index of the account in the account List. Returns -1 if the account was not found
         */
        public int searchForCustomerAccount(int accountNum)
        {
            int accountIndex = -1;
            foreach (Account account in thisCustomersAccounts)
            {
                accountIndex++;
                if (account.getAccountNumber() == accountNum)
                {
                    return accountIndex;
                }
            }
            return -1;
        }


        /**
         * Calculates the interest on the account
         * @return Decimal   The calculated interest
         */
        public Decimal calculateInterest(int index)
        {
            /* Balance * (interestRate / 100) */

            return thisCustomersAccounts[index].getBalance() * (Account.getInterestRate() / 100);
        }
    }
}
