
using System;


namespace dcld
{
	public class NumberBaseConverter
	{


        internal static bool IsHexValue(string input_string)
		{
			bool bres = false;
			string str_dummy = "";

			str_dummy = input_string;
			str_dummy = str_dummy.ToLower();
			str_dummy = str_dummy.Trim();
			bres = (str_dummy.StartsWith("0x") ||
					str_dummy.Contains("a") || str_dummy.Contains("b") || str_dummy.Contains("c") ||
					str_dummy.Contains("d") || str_dummy.Contains("e") || str_dummy.Contains("freq"));

			return bres;
		}

        internal static bool IsBinValue(string input_string)
		{
			bool bres = false;
			string str_dummy = "";
            int i = 0;

			str_dummy = input_string.Trim();
			str_dummy = str_dummy.ToLower();
			str_dummy = str_dummy.Trim();

            // test for binary format 
			bres = (str_dummy.StartsWith("0b"));

            // Search string for non-binary letters
            for (i = 0; i < str_dummy.Length; i++)
            {
                bres &= ((str_dummy.Substring(i, 1) == "0") || (str_dummy.Substring(i, 1) == "1") || (str_dummy.Substring(i, 1) == "1") || (str_dummy.Substring(i, 1) == " "));
            }

                return bres;
		}

        internal static bool IsFractionalValue(string input_string)
		{
			bool bres = false;
			string str_dummy = "";
            double dbl_dummy = 0.0;

			str_dummy = input_string;
			str_dummy = str_dummy.ToLower();
			str_dummy = str_dummy.Trim();
			bres = (str_dummy.Contains(".") || str_dummy.Contains(","));

            try
            {
                if (bres)
                {
                    dbl_dummy = Convert.ToDouble(input_string);
                    bres = ((1 > dbl_dummy) && (dbl_dummy >= -1));
                }
                return bres;

            }
            catch
            {
                return (false);
            }
		}

        internal static bool IsFloatingPointValue(string input_string)
        {
            bool bres = false;
            string str_dummy = "";
            double dbl_dummy = 0.0;

            str_dummy = input_string;
            str_dummy = str_dummy.ToLower();
            str_dummy = str_dummy.Trim();
            bres = (str_dummy.Contains(".") || str_dummy.Contains(","));

            try
            {
                if (bres)
                {
                    dbl_dummy = Convert.ToDouble(input_string);
                }
                return bres;

            }
            catch
            {
                return (false);
            }
        }

        /*
        internal static UInt16 Any2Dec16b(string input_value)
		{
			int i = 0;
			bool isHex = false, isFractional = false, isUnsigned = false, isSigned = false, isBinary = false; 
			string str_dummy = "", str_buf = "";

			str_dummy = input_value;
			str_dummy = str_dummy.Trim();
			str_dummy = str_dummy.ToLower();

			if (str_dummy.Contains(" "))
			{ // remove additional SPACE characters
				for (i = 0; i < str_dummy.Length; i++)
				{
					if (!(Convert.ToChar(str_dummy.Substring(i, 1)) == ' ')) str_buf = str_buf + str_dummy.Substring(i, 1);
				}
				str_dummy = str_buf;
			}

			str_dummy = str_dummy.Replace(Convert.ToChar(32), Convert.ToChar(0));
			if (str_dummy.Length == 0) str_dummy = "0";
			else if ((str_dummy == "-") || (str_dummy == ".") || (str_dummy == ",")) return (0);
			if (str_dummy.StartsWith(".")) str_dummy = "0" + str_dummy;
			if (str_dummy.StartsWith(",")) str_dummy = "0" + str_dummy;

			// check if input number is already in hexadezimal format
			isHex = (str_dummy.StartsWith("0x"));														// check if input number is hex number
			isBinary = (str_dummy.StartsWith("0b"));													// check if input number is binary number
			isFractional = ((str_dummy.Contains(".")) || (str_dummy.Contains(".")));					// check if input numer is in fractional format
			isSigned = ((str_dummy.Contains("-")) && (isFractional) && (!isHex) && (!isBinary));		// check if input number is signed integer
			isUnsigned = ((!str_dummy.Contains("-")) && (!isFractional) && (!isHex) && (!isBinary)); 	// check if input number is unsigned integer

			// mixed, non-sensical format
			if ((str_dummy.Contains("-") || str_dummy.Contains(".") || str_dummy.Contains(",")) &&
				(str_dummy.Contains("a") || str_dummy.Contains("b") || str_dummy.Contains("c") || str_dummy.Contains("d") || str_dummy.Contains("e") || str_dummy.Contains("freq") || str_dummy.Contains("x")) ||
				((str_dummy.Contains("x") && !str_dummy.StartsWith("0x")))
			   )
				return (0);

			// Convert input string into int32 if input format was different
			if (isHex) str_dummy = NumberBaseConverter.Hex2Dec(str_dummy, 64, false).ToString();
//			else if (isBinary) str_dummy = Convert.ToString(NumberBaseConverter.Bin2Dec(str_dummy, false));
//			else if (isSigned && !isFractional) str_dummy = Convert.ToString(NumberBaseConverter.SignedDec2UnsignedDec(Convert.ToInt32(str_dummy)));
//			else if (isFractional) str_dummy = Convert.ToString(NumberBaseConverter.Q152Dec16b(Convert.ToDouble(str_dummy)));

//			if (Convert.ToInt32(str_dummy) < 0)	str_dummy = Convert.ToString(NumberBaseConverter.SignedDec2UnsignedDec(Convert.ToInt32(str_dummy)));

			return Convert.ToUInt16(str_dummy);

		}
        */

        // singed int to unsigned int
        internal static UInt64 SignedDec2UnsignedDec(long input_value, int bit_len)
		{
			Int64 int_dummy = 0;
            Int64 Qmax = 0;

            int_dummy = input_value;

            if ((Math.Floor((double)bit_len / 8.0)) == ((double)bit_len / 8.0))
            { // bit_len is even
                Qmax = Convert.ToInt64(Math.Pow(2, bit_len - 1));
            }
            else
            { // bit_len in odd
                Qmax = Convert.ToInt64(Math.Pow(2, bit_len));
            }

            if (int_dummy > (Qmax - 1)) int_dummy = (Qmax - 1);
            if (int_dummy < -Qmax) int_dummy = -Qmax;
            if (int_dummy < 0) { int_dummy += (2 * Qmax); }

            return Convert.ToUInt64(int_dummy); 
		}

        // Unsigned integer to signed integer
        internal static Int64 UnsignedDec2SignedDec(long input_value, int bit_len)
		{
            Int64 int_dummy = 0;
            Int64 Qmax = 0;

            int_dummy = input_value;

            if ((Math.Floor((double)bit_len / 8.0)) == ((double)bit_len / 8.0))
            { // bit_len is even
                Qmax = Convert.ToInt64(Math.Pow(2, bit_len));
            }
            else
            { // bit_len in odd
                Qmax = Convert.ToInt64(Math.Pow(2, bit_len + 1));
            }

            if (int_dummy > (Qmax - 1)) int_dummy = (Qmax - 1);
            if (int_dummy < -Qmax) int_dummy = -Qmax;
            if (int_dummy >= (Qmax >> 1)) { int_dummy -= Qmax; }

            return Convert.ToInt64(int_dummy);
        }

        // Integer to formated Hexadezimal
        internal static string Dec2Hex(long val, int bit_len, bool lead_zeros, bool lead_format)
        {
            string str_dummy = "";
            int hex_digits = 0, i = 0;

            str_dummy = val.ToString("X");
            hex_digits = Convert.ToInt32(Math.Ceiling((double)bit_len / 4.0));

            if (str_dummy.Length > hex_digits)
            {
                str_dummy = str_dummy.Substring((str_dummy.Length - hex_digits), hex_digits);
            }

            if (lead_zeros)
            {
                for (i = str_dummy.Length; i < hex_digits; i++)
                {
                    str_dummy = "0" + str_dummy;
                }
            }

            if (lead_format)
            {
                str_dummy = "0x" + str_dummy.Trim();
            }

            return str_dummy;
        }

        // Integer into formated Binary
        internal static string Dec2Bin(long input_value, int bit_len, bool space_format, bool lead_format)
        {
            int i = 0;
            string str_dummy = "", bit_string = "";

            try
            {
                str_dummy = Convert.ToString(input_value, 2).PadLeft(bit_len, '0');
                if (str_dummy.Length > bit_len) str_dummy = str_dummy.Substring(str_dummy.Length - bit_len, bit_len);

                if (space_format)
                {
                    for (i = Convert.ToInt32(Math.Ceiling((double)bit_len / 8.0)); i > 0; i--)
                    {
                        bit_string = bit_string + " " + str_dummy.Substring(str_dummy.Length - (i * 8), 8);
                    }
                }
                else
                {
                    bit_string = str_dummy;
                }

                if (lead_format)
                {
                    bit_string = "0b" + bit_string.Trim();
                }

                return bit_string.Trim();
            }
            catch { return ("NaN"); }
        }

        // Integer to Fractional
        internal static double Dec2Fractional(long input_value, int Q_bit_len)
		{
			double dbl_dummy = 0;
            Int64 int_dummy = 0, Qmax = 0;

            try
            {

                Qmax = Convert.ToInt64(Math.Pow(2, Q_bit_len));
                int_dummy = input_value;

                if (int_dummy < -Qmax) int_dummy = -Qmax;
                if (int_dummy > (Qmax - 1)) int_dummy -= (Qmax << 1);
                dbl_dummy = (Convert.ToDouble(int_dummy) * Math.Pow(2, -Q_bit_len));

                return Convert.ToDouble(dbl_dummy);
            }
            catch
            {
                return 0.0;
            }
		}

        // Fractional to Integer
        internal static Int64 Fractional2Dec(double input_value, int Q_bit_len, bool int_signed)
		{
			Int64 int_dummy = 0;
            int Qmax = 0;

            if ((int_signed) && ((Math.Floor((double)Q_bit_len / 8.0)) == ((double)Q_bit_len / 8.0)) )
                { Qmax = Q_bit_len - 1; }
            else
                { Qmax = Q_bit_len; }

            int_dummy = Convert.ToInt64(input_value * Math.Pow(2, Qmax));

            if ((!int_signed) && (int_dummy < 0))
            {
                int_dummy += (int)Math.Pow(2, (Qmax + 1));
            }

			return Convert.ToInt64(int_dummy);
		}

        // Fractional into formated Hexadecimal
        internal static string Fractional2Hex(double input_value, int Q_bit_len, int hex_bit_len, bool lead_zeros, bool lead_format)
        {
            string strHex = "";
            Int64 int_dummy = 0;
            Int64 Qmax = 0;
            bool signed_int = false;

            signed_int = ((Math.Floor((double)Q_bit_len / 8.0)) != ((double)Q_bit_len / 8.0));

            int_dummy = Fractional2Dec(input_value, Q_bit_len, signed_int);

            Qmax = Convert.ToInt64(Math.Pow(2, Q_bit_len));

            if (int_dummy < -Qmax) int_dummy = -Qmax;
            if (int_dummy > (Qmax - 1)) int_dummy = (Qmax - 1);

            strHex = Dec2Hex(int_dummy, hex_bit_len, lead_zeros, lead_format);

            return (strHex);
        }

        // Fractional into formated Binary
        internal static string Fractional2Bin(double input_value, int Q_bit_len, int bin_bit_len, bool lead_zeros, bool lead_format)
        {
            string strBin = "";
            Int64 int_dummy = 0;
            Int64 Qmax = 0;
            bool signed_int = false;

            signed_int = ((Math.Floor((double)Q_bit_len / 8.0)) != ((double)Q_bit_len / 8.0));

            int_dummy = Fractional2Dec(input_value, Q_bit_len, signed_int);

            Qmax = Convert.ToInt64(Math.Pow(2, Q_bit_len));

            if (int_dummy < -Qmax) int_dummy = -Qmax;
            if (int_dummy > (Qmax - 1)) int_dummy = (Qmax - 1);

            strBin = Dec2Bin((int)int_dummy, bin_bit_len, lead_zeros, lead_format);

            return (strBin);
        }

        // Hexadezimal to Integer
        internal static Int64 Hex2Dec(string input_string, int int_bit_len, bool signed_int)
		{
			string str_dummy = "";
			Int64 int_dummy = 0;

			str_dummy = input_string;

            // remove spaces
            str_dummy = str_dummy.Trim();
            str_dummy = str_dummy.ToLower();

            // if input string is empty, skip conversion here
            if (str_dummy.Length == 0) str_dummy = "0";

            // Convert value into HEX format without leading "0x"
            str_dummy = str_dummy.Substring(str_dummy.IndexOf("x") + 1);
            str_dummy = str_dummy.Trim();

            try
            {
                if (signed_int)
                {
                    int_dummy = Convert.ToInt64(str_dummy, 16);
                    int_dummy = UnsignedDec2SignedDec(int_dummy, int_bit_len);
                }
                else int_dummy = (Int64)Convert.ToUInt64(str_dummy, 16);

                return Convert.ToInt64(int_dummy);
            }
            catch { return (0); }
		}

        // Hexadezimal to formated Binary
        internal static string Hex2Bin(string input_string, int bin_bit_len, bool space_format, bool lead_format)
        {
            string str_dummy = "", strBin = "";
            Int64 int_dummy = 0;

            str_dummy = input_string;

            // remove spaces
            str_dummy = str_dummy.Trim();
            str_dummy = str_dummy.ToLower();

            // if input string is empty, skip conversion here
            if (str_dummy.Length == 0) str_dummy = "0";

            // Convert value into HEX format without leading "0x"
            str_dummy = str_dummy.Substring(str_dummy.IndexOf("x") + 1);
            str_dummy = str_dummy.Trim();

            try
            {
                int_dummy = (Int64)Convert.ToUInt64(str_dummy, 16);
                strBin = Dec2Bin((int)int_dummy, bin_bit_len, space_format, lead_format);

                return (strBin);
            }
            catch { return ("NAN"); }
        }

        // Hexadezimal to Fractional
        internal static double Hex2Fractional(string input_string, int Q_bit_len)
        {
            string str_dummy = "";
            Int64 int_dummy = 0;
            double dbl_dummy = 0.0;

            str_dummy = input_string;

            // remove spaces
            str_dummy = str_dummy.Trim();
            str_dummy = str_dummy.ToLower();

            // if input string is empty, skip conversion here
            if (str_dummy.Length == 0) str_dummy = "0";

            // Convert value into HEX format without leading "0x"
            str_dummy = str_dummy.Substring(str_dummy.IndexOf("x") + 1);
            str_dummy = str_dummy.Trim();

            try
            {
                int_dummy = (Int64)Convert.ToUInt64(str_dummy, 16);
                dbl_dummy = Dec2Fractional((long)int_dummy, Q_bit_len);

                return (dbl_dummy);
            }
            catch { return (0.0); }
        }

        // Binary to Hexadezimal
        internal static string Bin2Hex(string strBin, int bit_len, bool lead_zeros, bool lead_format)
		{
            long decNumber = (long)Bin2Dec(strBin, false);
            return Dec2Hex(decNumber, bit_len, lead_zeros, lead_format);
		}

        // Binary to Integer
        internal static Int64 Bin2Dec(string strBin, bool signed_int)
		{
			Int64 int_dummy = 0;
			
			strBin = strBin.Trim();
			strBin = strBin.ToLower();
            strBin = strBin.Substring((strBin.IndexOf("b") + 1), strBin.Length - (strBin.IndexOf("b") + 1));
            strBin = strBin.Replace(" ", "");
            strBin = strBin.Trim();

            if ((signed_int) && (strBin.Substring(0,1) == "1"))
            { 
                int_dummy = Convert.ToInt64(strBin, 2);
                int_dummy -= (Int64)Math.Pow(2.0, (double)strBin.Length);
            }
            else { int_dummy = (Int64)Convert.ToUInt64(strBin, 2); }
			
			return int_dummy;
		}

        // Binary to Fractional
        internal static double Bin2Fractional(string strBin, int Q_bit_len)
        {
            long decNumber = (long)Bin2Dec(strBin, false);
            return Dec2Fractional(decNumber, Q_bit_len);
        }

    }
}
