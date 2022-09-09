using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHS_lab1
{
    package andjox1;

    import java.math.BigDecimal;

    /**
     * This class defines the bank accounts.
     * @author Anders Johansson, andjox-1
     */
    public class Account
    {
        private static int countingAllAccountNumbers = 1000;
        private static final BigDecimal INTEREST_RATE = new BigDecimal(1.2);
        private static final String ACCOUNT_TYPE = "Sparkonto";
    private int accountNumber;
        private BigDecimal balance = new BigDecimal(0);


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
            balance = balance.add(new BigDecimal(amount));
        }

        /**
         * Calculates the balance after a withdrawal
         * @param amount    The amount of money to withdraw
         */
        public void withdraw(int amount)
        {
            balance = balance.subtract(new BigDecimal(amount));
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
         * @return BigDecimal   The account balance
         */
        public BigDecimal getBalance()
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
         * @return BigDecimal   The interest that all accounts have
         */
        public static BigDecimal getInterestRate()
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
