using static System.Console;

public class UX
{
    private readonly Banco _banco;
    private readonly string _titulo;

    public UX(string titulo, Banco banco)
    {
        _titulo = titulo;
        _banco = banco;
    }

    public void Executar()
    {
        CriarTitulo(_titulo);
        WriteLine(" [1] Criar Conta");
        WriteLine(" [2] Listar Contas");
        WriteLine(" [3] Efetuar Saque");
        WriteLine(" [4] Efetuar Depósito");
        WriteLine(" [5] Aumentar Limite");
        WriteLine(" [6] Diminuir Limite");
        ForegroundColor = ConsoleColor.Red;
        WriteLine("\n [9] Sair");
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
        ForegroundColor = ConsoleColor.Yellow;
        Write(" Digite a opção desejada: ");
        var opcao = ReadLine() ?? "";
        ForegroundColor = ConsoleColor.White;
        
        switch (opcao)
        {
            case "1": CriarConta(); break;
            case "2": MenuListarContas(); break;
            case "3": RealizarSaque(); break;
            case "4": RealizarDeposito(); break;
            case "5": AlterarLimite(aumentar: true); break;
            case "6": AlterarLimite(aumentar: false); break;
        }

        if (opcao != "9")
        {
            Executar();
        }
        else
        {
            // Salva ao sair
            _banco.SaveContas();
        }
    }

    private void CriarConta()
    {
        CriarTitulo(_titulo + " - Criar Conta");
        Write(" Numero:  ");
        var numero = Convert.ToInt32(ReadLine());
        Write(" Cliente: ");
        var cliente = ReadLine() ?? "";
        Write(" CPF:     ");
        var cpf = ReadLine() ?? "";
        Write(" Senha:   ");
        var senha = ReadLine() ?? "";
        Write(" Limite:  ");
        var limite = Convert.ToDecimal(ReadLine());

        var conta = new Conta(numero, cliente, cpf, senha, limite);
        _banco.Contas.Add(conta);

        CriarRodape("Conta criada com sucesso!");
    }

    private void MenuListarContas()
    {
        CriarTitulo(_titulo + " - Listar Contas");
        foreach (var conta in _banco.Contas)
        {
            WriteLine($" Conta: {conta.Numero} - {conta.Cliente}");
            WriteLine($" Saldo: {conta.Saldo:C} | Limite: {conta.Limite:C}");
            WriteLine($" Disponível: {conta.SaldoDisponivel:C}\n");
        }
        CriarRodape();
    }

    // --- NOVAS FUNÇÕES DE INTERAÇÃO ---

    private Conta? BuscarConta()
    {
        Write(" Digite o número da conta: ");
        if (int.TryParse(ReadLine(), out int numero))
        {
            var conta = _banco.Contas.FirstOrDefault(c => c.Numero == numero);
            if (conta == null)
            {
                WriteLine(" Conta não encontrada!");
            }
            return conta;
        }
        WriteLine(" Número inválido.");
        return null;
    }

    private void RealizarSaque()
    {
        CriarTitulo(_titulo + " - Saque");
        var conta = BuscarConta();
        if (conta != null)
        {
            Write(" Valor do saque: ");
            if (decimal.TryParse(ReadLine(), out decimal valor))
            {
                if (conta.Sacar(valor))
                    WriteLine(" Saque realizado com sucesso!");
                else
                    WriteLine(" Saldo insuficiente!");
            }
            else WriteLine(" Valor inválido.");
        }
        CriarRodape();
    }

    private void RealizarDeposito()
    {
        CriarTitulo(_titulo + " - Depósito");
        var conta = BuscarConta();
        if (conta != null)
        {
            Write(" Valor do depósito: ");
            if (decimal.TryParse(ReadLine(), out decimal valor))
            {
                conta.Depositar(valor);
                WriteLine(" Depósito realizado com sucesso!");
            }
            else WriteLine(" Valor inválido.");
        }
        CriarRodape();
    }

    private void AlterarLimite(bool aumentar)
    {
        string acao = aumentar ? "Aumentar" : "Diminuir";
        CriarTitulo($"{_titulo} - {acao} Limite");
        
        var conta = BuscarConta();
        if (conta != null)
        {
            Write($" Valor para {acao.ToLower()} o limite: ");
            if (decimal.TryParse(ReadLine(), out decimal valor))
            {
                if (aumentar) conta.AumentarLimite(valor);
                else conta.DiminuirLimite(valor);
                
                WriteLine(" Limite atualizado com sucesso!");
            }
            else WriteLine(" Valor inválido.");
        }
        CriarRodape();
    }

    private void CriarLinha() => WriteLine("-------------------------------------------------");

    private void CriarTitulo(string titulo)
    {
        Clear();
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
        ForegroundColor = ConsoleColor.Yellow;
        WriteLine(" " + titulo);
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
    }

    private void CriarRodape(string? mensagem = null)
    {
        CriarLinha();
        ForegroundColor = ConsoleColor.Green;
        if (mensagem != null) WriteLine(" " + mensagem);
        Write(" ENTER para continuar");
        ForegroundColor = ConsoleColor.White;
        ReadLine();
    }
}
