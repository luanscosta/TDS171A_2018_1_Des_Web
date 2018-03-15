# TDS171A_2018_1_Des_Web

<h2>Injeção de Dependência (DI) no ASP.NET Core</h2>

É um padrão de projeto que visa facilitar o desacoplamento de componentes, tornando o projeto modular. Nesse padrão a classe que utilizara o recurso não o cria, mas sim o recebe em seu construtor. Essa classe define nos parâmetros do construtor a interface que o objeto a ser injetado tem implementada. Isso facilita a troca do recurso caso seja necessário, pois a outra classe a ser injetada possuirá os mesmos métodos com as mesamas assinaturas.

Essas dependências tem de ter seu tempo de vida estimado no container que a registra. É possível utilizar 3 métodos para configurar esse tempo de vida:
-Transient:
  É criada uma nova para cada controller e cada service.
-Scoped:
  Toda vez que uma nova request chega na controller, a dependência injetada é criada, e permanece viva até aquela request ser respondida.
-Singleton:
  São criadas uma única vez, e a instância criada será utilizada para todos os controller e services que a injetaram.
