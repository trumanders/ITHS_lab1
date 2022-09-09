using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHS_lab1
{
    package andjox1;
    import java.math.BigDecimal;
    import java.text.NumberFormat;
    import java.util.ArrayList;
    import java.util.Locale;

    /**
     * This class handles all of the customers and has methods to extract
     * information from customer  object. It has methods to error check
     * the input and also get information about the accounts indirectly.
     * @author Anders Johansson, andjox-1
     */
    public class BankLogic
    {
        private ArrayList<Customer> allCustomers = new ArrayList<>();

        /**
         * Returns a String ArrayList with all customers' persNum and full name.
         */
        public ArrayList<String> getAllCustomers()
        {
            ArrayList<String> allCustomersPersNumAndName = new ArrayList<>();
            for (Customer customer : allCustomers)
            {
                allCustomersPersNumAndName.add(customer.getPERSONAL_NUMBER() + " " + customer.getFullName());
            }
            return allCustomersPersNumAndName;
        }

        /**
         * Creates a new customer object. Checks if personal number is unique.
         * @param name      Customer's first name
         * @param surname   Customer's last name
         * @param pNo       Customer's personal number
         */
        public boolean createCustomer(String name, String surname, String pNo)
        {
            // Check if personal number IS VALID (does not already exists)
            if (isPersonalNumberValid(pNo))
            {

                /* If personal number is valid, create the customer and add to the ArrayList */
                allCustomers.add(new Customer(name, surname, pNo));
                return true;
            }
            return false;
        }


        /**
         * Retuns an ArrayList containing the customer's name, personal number, and it's accounts
         * and information about the customer's accounts.
         * @param pNo   Customer's personal number
         * @return      Info about the customer
         */
        public ArrayList<String> getCustomer(String pNo)
        {
            /* Search for the customer using the personal number pNo */
            int customerIndex = searchForCustomer(pNo);
            if (customerIndex < 0)
            {
                return null;
            }

            /* ArrayList to hold customer info to be returned */
            ArrayList<String> customerInfo = new ArrayList<>();

            /* Add customer name as the first element of the customerInfo */
            customerInfo.add(pNo + " " + allCustomers.get(customerIndex).getFullName());

            /* Then add the information about each account. */
            for (int i = 0; i < allCustomers.get(customerIndex).getNumberOfAccounts(); i++)
            {
                String accountNumber = Integer.toString(allCustomers.get(customerIndex).getAccountNumber(i));
                String formatBalance = formatMoneyString(allCustomers.get(customerIndex).getAccountBalance(i));
                String formatInterestRate = formatPercentString(Account.getInterestRate());
                customerInfo.add(accountNumber + " " + formatBalance + " " + Account.getACCOUNT_TYPE() + " " + formatInterestRate);
            }
            return customerInfo;
        }


        /**
         * Method to change a customer's name
         * @param name      The customer's new first name
         * @param surname   The customer's new last name
         * @param pNo       The pesonal number of the customer to change it's name
         * @return boolean  false: if both strings passed are empty, or if the customer was not found
         *                  true: if the operation was successful
         */
        public boolean changeCustomerName(String name, String surname, String pNo)
        {
            if (name.length() == 0 && surname.length() == 0)
            {
                return false;
            }
            /* Search for the customer to change name */
            int customerIndex = searchForCustomer(pNo);
            if (customerIndex < 0)
            {
                return false;
            }
            if (name.length() > 0) allCustomers.get(customerIndex).setfName(name);
            if (surname.length() > 0) allCustomers.get(customerIndex).setlName(surname);
            return true;
        }


        /**
         * Create a new account for a specified customer
         * @param pNo   The personal number of the customer to create a new account for
         * @return int  The account number of the created account
         */
        public int createSavingsAccount(String pNo)
        {
            int customerIndex = searchForCustomer(pNo);
            if (customerIndex < 0)
            {
                return -1;
            }
            allCustomers.get(customerIndex).addAccount();
            return Account.getCountingAllAccountNumbers();
        }


        /**
         * Gets information about a specific account: account number, balance, account type, interest rate
         * @param pNo           The personal number of the customer in possession of the account
         * @param accountId     The accont number for the account to get information about
         * @return String       A string containing the information, return null if the customer or acccount was not found
         */
        public String getAccount(String pNo, int accountId)
        {
            int customerIndex = searchForCustomerAndAccount(pNo, accountId)[0];
            int accountIndex = searchForCustomerAndAccount(pNo, accountId)[1];

            /* If customer or account doesn't exist */
            if (customerIndex < 0 || accountIndex < 0)
            {
                return null;
            }

            /* Get info */
            String accountNumber = Integer.toString(allCustomers.get(customerIndex).getAccountNumber(accountIndex));
            String balance = formatMoneyString(allCustomers.get(customerIndex).getAccountBalance(accountIndex));
            String accountType = Account.getACCOUNT_TYPE();
            String interestRate = formatPercentString(Account.getInterestRate());
            return accountNumber + " " + balance + " " + accountType + " " + interestRate;
        }


        /**
         * Performs a depoit to the specified account
         * @param pNo       The personal number of the customer
         * @param accountId The account number to make a deposit to
         * @param amount    The amount of money to transfer to the account
         * @return boolean  Returns true of the amount was > 0, both customer and it's account was found, and the deposit was made.
         */
        public boolean deposit(String pNo, int accountId, int amount)
        {
            if (amount < 0)
            {
                return false;
            }

            /* Look for customer and account */
            int customerIndex = searchForCustomerAndAccount(pNo, accountId)[0];
            int accountIndex = searchForCustomerAndAccount(pNo, accountId)[1];
            if (customerIndex < 0 || accountIndex < 0)
            {
                return false;
            }

            /* Make deposit */
            allCustomers.get(customerIndex).makeDeposit(amount, accountIndex);
            return true;
        }


        /**
         * Makes a withdrawal from an account
         * @param pNo       The customer's personal number
         * @param accountId The account number to make the withdrawal from
         * @param amount    The amount of money to withdraw
         * @return boolean  Returns true of the amount was > 0, both customer and it's account was found, and the withdrawal was made.
         */
        public boolean withdraw(String pNo, int accountId, int amount)
        {
            int customerIndex = searchForCustomerAndAccount(pNo, accountId)[0];
            int accountIndex = searchForCustomerAndAccount(pNo, accountId)[1];

            /* If amount is less than 0, or if customer or account doesn't exist */
            if (amount < 0 || (customerIndex < 0 || accountIndex < 0))
            {
                return false;
            }

            /* Check if there is enough money on the account */
            if (allCustomers.get(customerIndex).getAccountBalance(accountIndex).compareTo(new BigDecimal(amount)) >= 0)
            {
                allCustomers.get(customerIndex).makeWithdrawal(amount, accountIndex);
                return true;
            }
            return false;
        }


        /**
         * Closes an account and returns information abount the closed account
         * @param pNo       The personal number of the customer
         * @param accountId The account number for the account to be closed
         * @return String   Returns Accont number, balance, account tyoe, and interest before it is closed
         */
        public String closeAccount(String pNo, int accountId)
        {
            int customerIndex = searchForCustomerAndAccount(pNo, accountId)[0];
            int accountIndex = searchForCustomerAndAccount(pNo, accountId)[1];

            /* If customer or account does not exist */
            if (customerIndex < 0 || accountIndex < 0)
            {
                return null;
            }

            /* Get account info */
            String deletedAccountInfo = getDeletedAccountInfo(customerIndex, accountIndex);

            /* Delete the account */
            allCustomers.get(customerIndex).deleteAccount(accountIndex);
            return deletedAccountInfo;
        }


        /**
         * Removes a customer and it's accounts
         * @param pNo           The customers personal number
         * @return ArrayList    A list of information about the deleted customer and it's accounts
         */
        public ArrayList<String> deleteCustomer(String pNo)
        {
            /* Get the index of the customer to delete */
            int customerIndex = searchForCustomer(pNo);

            /* If personal number not found, return null, else start collecting info */
            if (customerIndex < 0)
            {
                return null;
            }
            else
            {
                ArrayList<String> deletedCustomerInfo = new ArrayList<>();

                /* Add personal number and name as the first element */
                deletedCustomerInfo.add(pNo + " " + allCustomers.get(customerIndex).getFullName());

                /* Add information about each account in the following elements of the ArrayList */
                for (int i = 0; i < allCustomers.get(customerIndex).getNumberOfAccounts(); i++)
                {

                    /* For each account, add the information */
                    deletedCustomerInfo.add(getDeletedAccountInfo(customerIndex, i));
                }

                /* Delete customer accounts */
                for (int i = 0; i < allCustomers.get(customerIndex).getNumberOfAccounts(); i++)
                {
                    allCustomers.get(customerIndex).deleteAccount(i);
                }

                /* Delete customer */
                allCustomers.remove(customerIndex);
                return deletedCustomerInfo;
            }
        }



        // CUSTOM METHODS //

        /**
         * Checks if the personal number of a new customer is valid
         * @param pNo       The personal number to be validated
         * @return boolean  Returns true if the personal number was valid (didn't alreay exist), else returns false
         */
        private boolean isPersonalNumberValid(String pNo)
        {
            boolean isValid = true;
            for (Customer customer : allCustomers)
            {
                if (customer.getPERSONAL_NUMBER().equals(pNo))
                {
                    return false;
                }
            }
            return true;
        }


        /**
         * Looks for a customer
         * @param pNo       The personal number to search for
         * @return int      The index of the customer in the customer ArrayList. Returns -1 if the customer was not found
         */
        private int searchForCustomer(String pNo)
        {
            int customerIndex = -1;
            for (Customer customer : allCustomers)
            {
                customerIndex++;
                if (customer.getPERSONAL_NUMBER().equals(pNo))
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
        private int[] searchForCustomerAndAccount(String pNo, int accountNumber)
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
            customerIndexAccountIndex[1] = allCustomers.get(customerIndexAccountIndex[0]).searchForCustomerAccount(accountNumber);
            return customerIndexAccountIndex;
        }


        /**
         * Formats a BigDecimal number as swedish money
         * @param money     The BigDecimal number to be formatted
         * @return String   The BigDecimal as a String
         */
        private String formatMoneyString(BigDecimal money)
        {
            return NumberFormat.getCurrencyInstance(new Locale("sv", "SE")).format(money);
        }


        /* Format the output of the interest rate */
        private String formatPercentString(BigDecimal interestRate)
        {
            NumberFormat percentFormat = NumberFormat.getPercentInstance(new Locale("sv", "SE"));
            percentFormat.setMaximumFractionDigits(1);
            return percentFormat.format(interestRate.divide(new BigDecimal(100)));
        }


        /**
         * Gets information about an account to be deleted
         * @param customerIndex     The index of the customer object to get the information from
         * @param accountIndex      The index of the account to get the information from
         * @return String           Account number, balance, account type, and interest
         */
        private String getDeletedAccountInfo(int customerIndex, int accountIndex)
        {
            String balance = formatMoneyString(allCustomers.get(customerIndex).getAccountBalance(accountIndex));
            String interest = NumberFormat.getCurrencyInstance(new Locale("sv", "SE")).format(allCustomers.get(customerIndex).calculateInterest(accountIndex));
            return allCustomers.get(customerIndex).getAccountNumber(accountIndex) + " " + balance + " " + Account.getACCOUNT_TYPE() + " " + interest;
        }
    }

}
