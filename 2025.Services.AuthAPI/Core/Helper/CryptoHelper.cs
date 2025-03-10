﻿using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace _2025.Services.AuthAPI.Core.Helper
{
    public static class CryptoHelper
    {
        //
        // Summary:
        //     Returns an RFC 2898 hash value for the specified password.
        //
        // Parameters:
        //   password:
        //     The password to generate a hash value for.
        //
        // Returns:
        //     The hash value for password as a base-64-encoded string.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     password is null.
        public static string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            byte[] salt;
            byte[] bytes;
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 16, 1000))
            {
                salt = rfc2898DeriveBytes.Salt;
                bytes = rfc2898DeriveBytes.GetBytes(32);
            }

            byte[] array = new byte[49];
            Buffer.BlockCopy(salt, 0, array, 1, 16);
            Buffer.BlockCopy(bytes, 0, array, 17, 32);
            return Convert.ToBase64String(array);
        }

        //
        // Summary:
        //     Determines whether the specified RFC 2898 hash and password are a cryptographic
        //     match.
        //
        // Parameters:
        //   hashedPassword:
        //     The previously-computed RFC 2898 hash value as a base-64-encoded string.
        //
        //   password:
        //     The plaintext password to cryptographically compare with hashedPassword.
        //
        // Returns:
        //     true if the hash value is a cryptographic match for the password; otherwise,
        //     false.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     hashedPassword or password is null.
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException("hashedPassword");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            byte[] array = Convert.FromBase64String(hashedPassword);
            if (array.Length != 49 || array[0] != 0)
            {
                return false;
            }

            byte[] array2 = new byte[16];
            Buffer.BlockCopy(array, 1, array2, 0, 16);
            byte[] array3 = new byte[32];
            Buffer.BlockCopy(array, 17, array3, 0, 32);
            byte[] bytes;
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, array2, 1000))
            {
                bytes = rfc2898DeriveBytes.GetBytes(32);
            }

            return ByteArraysEqual(array3, bytes);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            bool flag = true;
            for (int i = 0; i < a.Length; i++)
            {
                flag &= a[i] == b[i];
            }

            return flag;
        }
    }
}
//Beta
//0 / 10
//used queries
//1