using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHS_lab1
{
    package andjox1;
    import java.math.BigDecimal;
    import java.util.ArrayList;

    /**
     * This class defines the customers. It has methods to make deposit, withdrawal, calculate
     * interest, and search for a customer's accounts, and hand out information about a customer's
     * accounts.
     * @author Anders Johansson, andjox-1
     */
    public class Customer
    {
        private String fName;
        private String lName;
        private final String PERSONAL_NUMBER;

    /* Contains a list of the customer's accounts */
    private ArrayList<Account> thisCustomersAccounts = new ArrayList<>();

        public Customer(String fName, String lName, String persNum)
        {
            this.fName = fName;
            this.lName = lName;
            this.PERSONAL_NUMBER = persNum;
        }

        /* SETTERS */

        /**
         * Sets the customer's first name
         * @param fName     The customer's first name
         */
        public void setfName(String fName)
        {
            this.fName = fName;
        }

        /**
         * Sets the customer's last name
         * @param lName     The customer's last name
         */
        public void setlName(String lName)
        {
            this.lName = lName;
        }

        /**
         * Creates a new account in the account-ArrayList
         */
        public void addAccount()
        {
            this.thisCustomersAccounts.add(new Account());
        }


        /**
         * Deletes the account with the specified index in the account-ArrayList
         * @param index     The index of the account to be deleted
         */
        public void deleteAccount(int index)
        {
            thisCustomersAccounts.remove(index);
        }


        /**
         * The method calls the deposit method for the specified account
         * @param amount    The amount to pass to the deposit method
         * @param index     The index of the account
         */
        public void makeDeposit(int amount, int index)
        {
            thisCustomersAccounts.get(index).deposit(amount);
        }


        /**
         * Method to call the withdraw-method for the specified account
         * @param amount    The amount to pass to the withdraw method
         * @param index     The index of the account
         */
        public void makeWithdrawal(int amount, int index)
        {
            thisCustomersAccounts.get(index).withdraw(amount);
        }


        // GETTERS //

        /**
         * Get the customer's personal number
         * @return String   The customer's personal number
         */
        public String getPERSONAL_NUMBER()
        {
            return this.PERSONAL_NUMBER;
        }

        /**
         * Gets the customer's first name
         * @return String   The customer's first name
         */
        public String getfName()
        {
            return this.fName;
        }

        /**
         * Gets the customer's last name
         * @return String   The customer's last name
         */
        public String getlName()
        {
            return this.lName;
        }

        /**
         * Gets the customers first and last name.
         * @return String   The customer's first and last name separated by space.
         */
        public String getFullName()
        {
            return getfName() + " " + getlName();
        }


        /**
         * Gets the number of accounts of a customer
         * @return int  The number of accounts that the customer has
         */
        public int getNumberOfAccounts()
        {
            return thisCustomersAccounts.size();
        }


        /**
         * Gets the account numbers of a customer's accounts
         * @return ArrrayList<Integer>  List of all the account numbers
         */
        public int getAccountNumber(int index)
        {
            return thisCustomersAccounts.get(index).getAccountNumber();
        }


        /**
         * Gets the balance of all accounts of the customer
         * @return ArrayList<BigDecimal>    List of the balances of the accounts
         */
        public BigDecimal getAccountBalance(int index)
        {
            return thisCustomersAccounts.get(index).getBalance();
        }


        /**
         * Checks if an account exists and gets it's index in the account ArrayList
         * @param accountNum    The account number to look up
         * @return int          The index of the account in the account ArrayList. Returns -1 if the account was not found
         */
        public int searchForCustomerAccount(int accountNum)
        {
            int accountIndex = -1;
            for (Account account : thisCustomersAccounts)
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
         * @return BigDecimal   The calculated interest
         */
        public BigDecimal calculateInterest(int index)
        {
            /* Balance * (interestRate / 100) */

            return thisCustomersAccounts.get(index).getBalance().multiply(Account.getInterestRate().divide(new BigDecimal(100)));
        }
    }

}
