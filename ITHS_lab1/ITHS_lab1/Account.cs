using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHS_lab1
{
    class Account
    {
        /**
         * This class defines the bank accounts.
         * @author Anders Johansson, anders.johansson@iths.se
         */
        public class Account
        {
            private static int countingAllAccountNumbers = 1000;
            private const Decimal INTEREST_RATE = 1.2m;
            private const String ACCOUNT_TYPE = "Sparkonto";
            private int accountNumber;
            private Decimal balance = 0;


            /* Constructor */
            public Account()
            {
                countingAllAccountNumbers++;
                accountNumber = countingAllAccountNumbers;
            }


            /* TRANSACTIONS */

            /**
             * Calculates the new balance after a deposit
             * @param amount    The amount of money to deposit
             */
            public void deposit(int amount)
            {
                balance += amount;
            }


            /**
             * Calculates the balance after a withdrawal
             * @param amount    The amount of money to withdraw
             */
            public void withdraw(int amount)
            {
                balance -= amount;
            }


            /* GETTERS */

            /**
             * Gets the account number
             * @return int  The account number
             */
            public int getAccountNumber()
            {
                return this.accountNumber;
            }


            /**
             * Gets the balance od the account
             * @return Decimal   The account balance
             */
            public Decimal getBalance()
            {
                return balance;
            }


            /**
             * Gets the account type
             * @return String   The account type
             */
            public static String getACCOUNT_TYPE()
            {
                return ACCOUNT_TYPE;
            }


            /**
             * Gets the interest rate of all accounts (static)
             * @return Decimal   The interest that all accounts have
             */
            public static Decimal getInterestRate()
            {
                return INTEREST_RATE;
            }


            /**
             * Gets the account number of the last added account
             * @return int  The account number of the last added account
             */
            public static int getCountingAllAccountNumbers()
            {
                return countingAllAccountNumbers;
            }
        }
    }
}
