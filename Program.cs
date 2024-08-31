using System;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.VisualBasic;

namespace dotnetcore
{
    class Program
    {
        static void Main(string[] args)
        {
            var secretName = "MySecret";
            var keyVaultName = "my-vault-no-protect";
            var kvUri = "https://my-vault-no-protect.vault.azure.net";

            var options = new SecretClientOptions(){
                Retry = {
                    Delay = TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = Azure.Core.RetryMode.Exponential
                }
            };

            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential(), options);
            KeyVaultSecret secret = client.GetSecret(secretName);


            Console.WriteLine($"GetSecret: {secret.Value}");
            Console.Write("Enter Secret > ");

            var secretValue = Console.ReadLine();
            client.SetSecret(secretName, secretValue);

            Console.Write("SetSecret:");
            Console.Write($" Key: + {secretName}");
            Console.Write($" Value: + {secretValue}");
            Console.WriteLine($"GetSecret: {secret.Value}");

            client.StartDeleteSecret(secretName);
            Console.WriteLine($"StartDeleteSecret: {keyVaultName}");
            Console.WriteLine($"GetSecret: {secret.Value}");
        }
    }
}
