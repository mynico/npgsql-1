﻿#if !DNXCORE50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TlsClientStream
{
    internal class CipherSuiteInfo
    {
        public CipherSuite Id;
        public KeyExchange KeyExchange;
        public int AesKeyLen;
        public TLSHashAlgorithm HashAlgorithm;
        public PRFAlgorithm PRFAlgorithm;
        public AesMode AesMode;

        public CipherSuiteInfo()
        {
            PRFAlgorithm = PRFAlgorithm.TLSPrfSHA256;
            AesMode = AesMode.CBC;
        }

        public int MACLen
        {
            get
            {
                return Utils.GetHashLen(HashAlgorithm);
            }
        }

        public HMAC CreateHMAC(byte[] key)
        {
            switch (HashAlgorithm)
            {
                case TLSHashAlgorithm.SHA1:
                    return new HMACSHA1(key);
                case TLSHashAlgorithm.SHA256:
                    return new HMACSHA256(key);
                case TLSHashAlgorithm.SHA384:
                    return new HMACSHA384(key);
                case TLSHashAlgorithm.SHA512:
                    return new HMACSHA512(key);
                default:
                    throw new NotSupportedException();
            }
        }

        public HMAC CreatePrfHMAC(byte[] key)
        {
            switch (PRFAlgorithm)
            {
                case PRFAlgorithm.TLSPrfSHA256:
                    return new HMACSHA256(key);
                case PRFAlgorithm.TLSPrfSHA384:
                    return new HMACSHA384(key);
                default:
                    throw new NotSupportedException();
            }
        }

        public SignatureAlgorithm GetSignatureAlgorithm()
        {
            switch (KeyExchange)
            {
                case KeyExchange.DHE_DSS:
                    return SignatureAlgorithm.DSA;
                case KeyExchange.DHE_RSA:
                case KeyExchange.ECDH_RSA:
                case KeyExchange.ECDHE_RSA:
                    return SignatureAlgorithm.RSA;
                case KeyExchange.ECDH_ECDSA:
                case KeyExchange.ECDHE_ECDSA:
                    return SignatureAlgorithm.ECDSA;
                case KeyExchange.RSA:
                default:
                    throw new NotSupportedException();
            }
        }

        public bool IsAllowedBefore1_2 { get { return AesMode == AesMode.CBC && (ushort)Id < 0xC023; } }

        public static readonly CipherSuiteInfo[] Supported = new CipherSuiteInfo[] {
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA384, KeyExchange = KeyExchange.ECDHE_RSA, AesKeyLen = 256, HashAlgorithm = TLSHashAlgorithm.SHA384, PRFAlgorithm = PRFAlgorithm.TLSPrfSHA384 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA, KeyExchange = KeyExchange.ECDHE_RSA, AesKeyLen = 256, HashAlgorithm = TLSHashAlgorithm.SHA1 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA256, KeyExchange = KeyExchange.ECDHE_RSA, AesKeyLen = 128, HashAlgorithm = TLSHashAlgorithm.SHA256 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA, KeyExchange = KeyExchange.ECDHE_RSA, AesKeyLen = 128, HashAlgorithm = TLSHashAlgorithm.SHA1 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDHE_ECDSA_WITH_AES_256_CBC_SHA384, KeyExchange = KeyExchange.ECDHE_ECDSA, AesKeyLen = 256, HashAlgorithm = TLSHashAlgorithm.SHA384, PRFAlgorithm = PRFAlgorithm.TLSPrfSHA384 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDHE_ECDSA_WITH_AES_128_CBC_SHA256, KeyExchange = KeyExchange.ECDHE_ECDSA, AesKeyLen = 128, HashAlgorithm = TLSHashAlgorithm.SHA256 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_DHE_RSA_WITH_AES_256_CBC_SHA256, KeyExchange = KeyExchange.DHE_RSA, AesKeyLen = 256, HashAlgorithm = TLSHashAlgorithm.SHA256 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_DHE_RSA_WITH_AES_256_CBC_SHA, KeyExchange = KeyExchange.DHE_RSA, AesKeyLen = 256, HashAlgorithm = TLSHashAlgorithm.SHA1 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_DHE_RSA_WITH_AES_128_CBC_SHA256, KeyExchange = KeyExchange.DHE_RSA, AesKeyLen = 128, HashAlgorithm = TLSHashAlgorithm.SHA256 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_DHE_RSA_WITH_AES_128_CBC_SHA, KeyExchange = KeyExchange.DHE_RSA, AesKeyLen = 128, HashAlgorithm = TLSHashAlgorithm.SHA1 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_DHE_DSS_WITH_AES_256_CBC_SHA256, KeyExchange = KeyExchange.DHE_DSS, AesKeyLen = 256, HashAlgorithm = TLSHashAlgorithm.SHA256 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_DHE_DSS_WITH_AES_256_CBC_SHA, KeyExchange = KeyExchange.DHE_DSS, AesKeyLen = 256, HashAlgorithm = TLSHashAlgorithm.SHA1 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_DHE_DSS_WITH_AES_128_CBC_SHA256, KeyExchange = KeyExchange.DHE_DSS, AesKeyLen = 128, HashAlgorithm = TLSHashAlgorithm.SHA256 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_DHE_DSS_WITH_AES_128_CBC_SHA, KeyExchange = KeyExchange.DHE_DSS, AesKeyLen = 128, HashAlgorithm = TLSHashAlgorithm.SHA1 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384, KeyExchange = KeyExchange.ECDHE_RSA, AesKeyLen = 256, AesMode = AesMode.GCM, PRFAlgorithm = PRFAlgorithm.TLSPrfSHA384 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256, KeyExchange = KeyExchange.ECDHE_RSA, AesKeyLen = 128, AesMode = AesMode.GCM },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDHE_ECDSA_WITH_AES_256_GCM_SHA384, KeyExchange = KeyExchange.ECDHE_ECDSA, AesKeyLen = 256, AesMode = AesMode.GCM, PRFAlgorithm = PRFAlgorithm.TLSPrfSHA384 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256, KeyExchange = KeyExchange.ECDHE_ECDSA, AesKeyLen = 128, AesMode = AesMode.GCM },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_DHE_RSA_WITH_AES_256_GCM_SHA384, KeyExchange = KeyExchange.DHE_RSA, AesKeyLen = 256, AesMode = AesMode.GCM, PRFAlgorithm = PRFAlgorithm.TLSPrfSHA384 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_DHE_RSA_WITH_AES_128_GCM_SHA256, KeyExchange = KeyExchange.DHE_RSA, AesKeyLen = 128, AesMode = AesMode.GCM },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_DHE_DSS_WITH_AES_256_GCM_SHA384, KeyExchange = KeyExchange.DHE_DSS, AesKeyLen = 256, AesMode = AesMode.GCM, PRFAlgorithm = PRFAlgorithm.TLSPrfSHA384 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_DHE_DSS_WITH_AES_128_GCM_SHA256, KeyExchange = KeyExchange.DHE_DSS, AesKeyLen = 128, AesMode = AesMode.GCM },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDH_RSA_WITH_AES_256_CBC_SHA384, KeyExchange = KeyExchange.ECDH_RSA, AesKeyLen = 256, HashAlgorithm = TLSHashAlgorithm.SHA384, PRFAlgorithm = PRFAlgorithm.TLSPrfSHA384 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDH_RSA_WITH_AES_256_CBC_SHA, KeyExchange = KeyExchange.ECDH_RSA, AesKeyLen = 256, HashAlgorithm = TLSHashAlgorithm.SHA1 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDH_RSA_WITH_AES_128_CBC_SHA256, KeyExchange = KeyExchange.ECDH_RSA, AesKeyLen = 128, HashAlgorithm = TLSHashAlgorithm.SHA256 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDH_RSA_WITH_AES_128_CBC_SHA, KeyExchange = KeyExchange.ECDH_RSA, AesKeyLen = 128, HashAlgorithm = TLSHashAlgorithm.SHA1 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDH_ECDSA_WITH_AES_256_CBC_SHA384, KeyExchange = KeyExchange.ECDH_ECDSA, AesKeyLen = 256, HashAlgorithm = TLSHashAlgorithm.SHA384, PRFAlgorithm = PRFAlgorithm.TLSPrfSHA384 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDH_ECDSA_WITH_AES_256_CBC_SHA, KeyExchange = KeyExchange.ECDH_ECDSA, AesKeyLen = 256, HashAlgorithm = TLSHashAlgorithm.SHA1 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDH_ECDSA_WITH_AES_128_CBC_SHA256, KeyExchange = KeyExchange.ECDH_ECDSA, AesKeyLen = 128, HashAlgorithm = TLSHashAlgorithm.SHA256 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDH_ECDSA_WITH_AES_128_CBC_SHA, KeyExchange = KeyExchange.ECDH_ECDSA, AesKeyLen = 128, HashAlgorithm = TLSHashAlgorithm.SHA1 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_RSA_WITH_AES_256_CBC_SHA256, KeyExchange = KeyExchange.RSA, AesKeyLen = 256, HashAlgorithm = TLSHashAlgorithm.SHA256 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_RSA_WITH_AES_256_CBC_SHA, KeyExchange = KeyExchange.RSA, AesKeyLen = 256, HashAlgorithm = TLSHashAlgorithm.SHA1 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_RSA_WITH_AES_128_CBC_SHA256, KeyExchange = KeyExchange.RSA, AesKeyLen = 128, HashAlgorithm = TLSHashAlgorithm.SHA256 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_RSA_WITH_AES_128_CBC_SHA, KeyExchange = KeyExchange.RSA, AesKeyLen = 128, HashAlgorithm = TLSHashAlgorithm.SHA1 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDH_RSA_WITH_AES_256_GCM_SHA384, KeyExchange = KeyExchange.ECDH_RSA, AesKeyLen = 256, AesMode = AesMode.GCM, PRFAlgorithm = PRFAlgorithm.TLSPrfSHA384 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDH_RSA_WITH_AES_128_GCM_SHA256, KeyExchange = KeyExchange.ECDH_RSA, AesKeyLen = 128, AesMode = AesMode.GCM },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDH_ECDSA_WITH_AES_256_GCM_SHA384, KeyExchange = KeyExchange.ECDH_ECDSA, AesKeyLen = 256, AesMode = AesMode.GCM, PRFAlgorithm = PRFAlgorithm.TLSPrfSHA384 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_ECDH_ECDSA_WITH_AES_128_GCM_SHA256, KeyExchange = KeyExchange.ECDH_ECDSA, AesKeyLen = 128, AesMode = AesMode.GCM },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_RSA_WITH_AES_256_GCM_SHA384, KeyExchange = KeyExchange.RSA, AesKeyLen = 256, AesMode = AesMode.GCM, PRFAlgorithm = PRFAlgorithm.TLSPrfSHA384 },
            new CipherSuiteInfo() { Id = CipherSuite.TLS_RSA_WITH_AES_128_GCM_SHA256, KeyExchange = KeyExchange.RSA, AesKeyLen = 128, AesMode = AesMode.GCM }
        };
    }
}
#endif