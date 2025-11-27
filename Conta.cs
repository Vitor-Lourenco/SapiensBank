using System.Text.Json.Serialization;

public class Conta
{
    public int Numero { get; set; }
    public string Cliente { get; set; }
    public string Cpf { get; set; }
    public string Senha { get; set; }
    public decimal Saldo { get; set; }
    public decimal Limite { get; set; }

    [JsonIgnore]
    public decimal SaldoDisponivel => Saldo + Limite;

    public Conta(int numero, string cliente, string cpf, string senha, decimal limite = 0)
    {
        Numero = numero;
        Cliente = cliente;
        Cpf = cpf;
        Senha = senha;
        Limite = limite;
    }

    // --- NOVOS MÉTODOS ---

    public void Depositar(decimal valor)
    {
        if (valor > 0)
        {
            Saldo += valor;
        }
    }

    public bool Sacar(decimal valor)
    {
        // Verifica se o valor do saque é maior que o saldo + limite
        if (valor > SaldoDisponivel)
        {
            return false; // Saldo insuficiente
        }
        
        Saldo -= valor;
        return true;
    }

    public void AumentarLimite(decimal valor)
    {
        if (valor > 0)
        {
            Limite += valor;
        }
    }

    public void DiminuirLimite(decimal valor)
    {
        if (valor > 0)
        {
            Limite -= valor;
            // Opcional: Impedir que o limite fique negativo se desejar
            if (Limite < 0) Limite = 0;
        }
    }
}