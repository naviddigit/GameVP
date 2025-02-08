// SoftEther VPN Server JSON-RPC Stub code for C#
// 
// VpnServerRpcTest.cs - Test sample code for SoftEther VPN Server JSON-RPC Stub
//
// This sample code shows how to call all available RPC functions.
// You can copy and paste test code to write your own C# codes.
//
// Automatically generated at 2019-07-10 14:36:11 by vpnserver-jsonrpc-codegen
//
// Licensed under the Apache License 2.0
// Copyright (c) 2014-2019 SoftEther VPN Project

using System;
using SoftEther.VPNServerRpc;
using System.Collections.Generic;
using System.Linq;

namespace SoftEther.Models
{
    public class VPNRPCT
    {
        VpnServerRpc api;

        public VPNRPCT(string vpnserver_host, int vpnserver_port, string admin_password, string hub_name = null)
        {
            api = new VpnServerRpc(vpnserver_host, vpnserver_port, admin_password, "");       // Speficy your VPN Server's password here.
        }

        public void CreateUser_WithoutPolicy(VpnRpcSetUser VpnRpcSetUser)
        {
            Console.WriteLine("Begin: Test_CreateUser");

            VpnRpcSetUser in_rpc_set_user = new VpnRpcSetUser()
            {
                HubName_str = "",
                Name_str = "test1",
                Realname_utf = "Cat man",
                Note_utf = "Hey!!!",
                AuthType_u32 = VpnRpcUserAuthType.Password,
                Auth_Password_str = "microsoft",
                Auth_UserCert_CertData = new byte[0] { },
                Auth_RootCert_Serial = new byte[0] { },
                Auth_RootCert_CommonName = "",
                Auth_Radius_RadiusUsername = "",
                Auth_NT_NTUsername = "",
                ExpireTime_dt = new DateTime(2019, 1, 1),


            };

            VpnRpcSetUser out_rpc_set_user = api.CreateUser(VpnRpcSetUser);
        }

        public void UpdateUser_WithoutPolicy(VpnRpcSetUser VpnRpcSetUser)
        {
            api.SetUser(VpnRpcSetUser);
        }


        public VpnRpcEnumUserItem[] ListUser(string hub_name)
        {
            VpnRpcEnumUser in_rpc_enum_user = new VpnRpcEnumUser()
            {
                HubName_str = hub_name,
            };
            return api.EnumUser(in_rpc_enum_user).UserList;
        }

        public VpnRpcSetUser GetUser(string username, string hub_name)
        {
            VpnRpcSetUser in_rpc_set_user = new VpnRpcSetUser()
            {
                HubName_str = hub_name,
                Name_str = username,

            };
            return api.GetUser(in_rpc_set_user);
        }

        public bool AnyUser(string username, string hub_name)
        {
            VpnRpcEnumUser in_rpc_enum_user = new VpnRpcEnumUser()
            {
                HubName_str = hub_name,
            };

            var GetList = api.EnumUser(in_rpc_enum_user).UserList;
            return GetList.Any(i => i.Name_str == username);
        }


        public object AnyUser_V1(string username, string hub_name)
        {
            VpnRpcSetUser in_rpc_set_user = new VpnRpcSetUser()
            {
                HubName_str = hub_name,
                Name_str = username,
            };

            try
            {
                VpnRpcSetUser out_rpc_set_user = api.GetUser(in_rpc_set_user);
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Code=29"))
                    return false;

                //throw new Exception(ex.Message);

                return ex.Message;

            }

        }


        public VpnRpcEnumSessionItem[] ListSession(string hub_name)
        {
            VpnRpcEnumSession in_rpc_enum_user = new VpnRpcEnumSession()
            {
                HubName_str = hub_name,
            };
            return api.EnumSession(in_rpc_enum_user).SessionList;
        }


        public void DeleteSesion(string hub_name, string name_str)
        {
            VpnRpcDeleteSession vpnRpcDeleteSession = new VpnRpcDeleteSession()
            {
                HubName_str = hub_name,
                Name_str = name_str
            };

            api.DeleteSession(vpnRpcDeleteSession);
        }


    }
}