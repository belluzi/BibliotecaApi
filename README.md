# BibliotecaApi - API de Gerenciamento de Biblioteca

Uma API REST para gerenciamento de bibliotecas, desenvolvida com ASP.NET Core 8.0. Permite cadastro de usuários, livros, empréstimos e cálculo de multas por atraso.

---

## Como Executar o Projeto

### Pré-requisitos

Você vai precisar ter instalado:
- **.NET 8 SDK** ou superior ([baixar aqui](https://dotnet.microsoft.com/download/dotnet/8.0))
- Um editor (VS Code, Visual Studio, etc) - opcional, mas recomendado

### Instalação

Clonar o repositório:
```bash
git clone https://github.com/belluzi/BibliotecaApi.git
cd BibliotecaApi
```

Restaurar as dependências:
```bash
dotnet restore
```

Compilar:
```bash
dotnet build
```

Executar:
```bash
dotnet run
```

Depois de iniciar, acesse e veja a documentação em `http://localhost:5023/swagger`

---

## Desafios Implementados

Todas as 8 tasks foram implementadas:

**TASK 1:** Validação de CPF (não permite duplicado)
- Endpoint: POST /Usuario/Cadastrar
- Status: OK

**TASK 2:** Validação de ISBN (exatamente 13 dígitos numéricos)
- Endpoint: POST /Livro/Cadastrar
- Status: OK

**TASK 3:** Correção de multa (sem multa se devolvido no prazo)
- Endpoint: POST /Emprestimo/Devolver
- Status: OK

**TASK 4:** Não permite emprestar livro que já está emprestado
- Endpoint: POST /Emprestimo/Cadastrar
- Status: OK

**TASK 5:** Listar todos os livros (rota pública)
- Endpoint: GET /Livro/Listar
- Status: OK

**TASK 6:** Bloqueia novo empréstimo se usuário tem atraso
- Endpoint: POST /Emprestimo/Cadastrar
- Status: OK

**TASK 7:** Lógica de multa escalonada
- Gera token em POST /Emprestimo/Devolver
- Lógica: primeiros 3 dias R$ 2,00/dia, depois R$ 3,50/dia, máximo R$ 50,00
- Status: OK

**TASK 8:** Credenciais para gerar token
- Email: admin@teste.com
- Senha: bibliotecaapiteste
- Status: OK

---

### 📊 Resumo de Implementação

| Task | Tipo | Descrição | Status |
|------|------|-----------|--------|
| 1 | Feature | Validar CPF | ✅ Implementada |
| 2 | Feature | Validar ISBN | ✅ Implementada |
| 3 | Bugfix | Corrigir Cálculo de Multa | ✅ Implementada |
| 4 | Bugfix | Livro Já Emprestado | ✅ Implementada |
| 5 | Feature | Listar Livros | ✅ Implementada |
| 6 | Feature | Usuário com Atraso | ✅ Implementada |
| 7 | Feature | Ajustar Regra de Multa | ✅ Implementada |
| 8 | Feature | Autenticação JWT | ✅ Implementada |

**Total: 8/8 desafios implementados (100%)**

---

## 📡 API Endpoints

### 🔐 Autenticação

#### Gerar Token JWT (Autenticação)

**Credenciais Padrão (Seed)**:
- Email: `admin@teste.com`
- Senha: `bibliotecaapiteste`

```http
POST /Auth/generate-token
Content-Type: application/json

{
  "email": "admin@teste.com",
  "senha": "bibliotecaapiteste"
}
```

**Response: 200 OK** ✅ (Credenciais Corretas)
```json
{
  "sucesso": true,
  "conteudo": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "mensagemErro": null
}
```

**Response: 401 Unauthorized** ❌ (Credenciais Inválidas)
```json
{
  "sucesso": false,
  "conteudo": null,
  "mensagemErro": "Email ou senha inválidos."
}
```

**Response: 400 Bad Request** ❌ (Campos Vazios)
```json
{
  "sucesso": false,
  "conteudo": null,
  "mensagemErro": "Erro ao gerar token: O email é obrigatório."
}
```

**Detalhes**:
- ✅ Sem autenticação (rota pública)
- ✅ Valida credenciais contra seed hardcoded
- ✅ Retorna token JWT válido por 60 minutos
- ✅ Validações: email e senha obrigatórios
- ✅ Claims do token: NameIdentifier (ID=1), Name (admin), Email (admin@teste.com), iat, exp
- ✅ Não utiliza banco de dados (seed em memória)

**Como Usar**:
1. Chamar `POST /Auth/generate-token` com email e senha corretos
2. Receber o token JWT na resposta
3. Usar o token em requisições posteriores: `Authorization: Bearer {token}`

---

### 👤 Usuários

#### Cadastrar Usuário
```http
POST /Usuario/Cadastrar
Authorization: Bearer {token}
Content-Type: application/json

{
  "nome": "João Silva",
  "cpf": "12345678901",
  "email": "joao@exemplo.com"
}
```

**Request Headers**:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json
```

**Response: 200 OK** ✅
```json
{
  "sucesso": true,
  "conteudo": 4,
  "mensagemErro": null
}
```

**Response: 400 Bad Request** ❌
```json
{
  "sucesso": false,
  "conteudo": 0,
  "mensagemErro": "Usuário com este CPF já está cadastrado."
}
```

**Response: 401 Unauthorized** ❌
```json
{
  "sucesso": false,
  "conteudo": null,
  "mensagemErro": "Token inválido ou expirado"
}
```

**Validações**:
- ✅ Nome obrigatório
- ✅ CPF com exatamente 11 dígitos
- ✅ CPF não pode ser duplicado no banco
- ✅ Requer autenticação JWT Bearer
- ✅ Retorna ID do usuário criado (conteudo)

**Status HTTP**:
- `200` → Cadastrado com sucesso
- `400` → CPF duplicado ou dados inválidos
- `401` → Token ausente ou inválido

---

### 📖 Livros

#### Cadastrar Livro
```http
POST /Livro/Cadastrar
Authorization: Bearer {token}
Content-Type: application/json

{
  "titulo": "Clean Code",
  "autor": "Robert Martin",
  "isbn": "0132350882343"
}
```

**Response: 200 OK** ✅
```json
{
  "sucesso": true,
  "conteudo": 5,
  "mensagemErro": null
}
```

**Response: 400 Bad Request** ❌
```json
{
  "sucesso": false,
  "conteudo": 0,
  "mensagemErro": "ISBN deve conter exatamente 13 dígitos numéricos."
}
```

**Response: 401 Unauthorized** ❌
```json
{
  "sucesso": false,
  "conteudo": null,
  "mensagemErro": "Unauthorized"
}
```

**Validações**:
- ✅ Título obrigatório
- ✅ Autor obrigatório
- ✅ ISBN com exatamente 13 dígitos numéricos
- ✅ ISBN não pode conter letras, hífens ou espaços
- ✅ Requer autenticação JWT Bearer
- ✅ Retorna ID do livro criado (conteudo)

**Status HTTP**:
- `200` → Livro criado com sucesso
- `400` → Dados inválidos (ISBN incorreto, campos vazios)
- `401` → Token ausente ou inválido

#### Listar Todos os Livros
```http
GET /Livro/Listar
Accept: application/json
```

**Response: 200 OK** ✅
```json
{
  "sucesso": true,
  "conteudo": [
    {
      "id": 1,
      "titulo": "O Senhor dos Anéis",
      "autor": "J.R.R. Tolkien",
      "isbn": "9780547928210"
    },
    {
      "id": 2,
      "titulo": "testando",
      "autor": "testando",
      "isbn": "1234567890987"
    }
  ],
  "mensagemErro": null
}
```

**Response: 200 OK (sem livros)** ✅
```json
{
  "sucesso": true,
  "conteudo": [],
  "mensagemErro": null
}
```

**Características**:
- ✅ Sem autenticação (rota pública)
- ✅ Retorna lista completa de livros
- ✅ Media type: application/json
- ✅ Cada livro contém: id, titulo, autor, isbn

**Status HTTP**:
- `200` → Livros retornados com sucesso (pode estar vazio)
- `400` → Erro na requisição

---

### 🏦 Empréstimos

#### Cadastrar Empréstimo
```http
POST /Emprestimo/Cadastrar
Authorization: Bearer {token}
Content-Type: application/json

{
  "idUsuario": 1,
  "idLivro": 1,
  "dataPrevistaDevolucao": "2026-03-25T11:55:04.622Z"
}
```

**Response: 200 OK** ✅
```json
{
  "sucesso": true,
  "conteudo": 15,
  "mensagemErro": null
}
```

**Response: 400 Bad Request** ❌ (Livro já emprestado)
```json
{
  "sucesso": false,
  "conteudo": 0,
  "mensagemErro": "Erro ao registrar empréstimo: Erro ao cadastrar empréstimo: Este livro já está emprestado e ainda não foi devolvido."
}
```

**Response: 400 Bad Request** ❌ (Usuário com atraso)
```json
{
  "sucesso": false,
  "conteudo": 0,
  "mensagemErro": "Usuário com empréstimo em atraso não pode realizar novo empréstimo."
}
```

**Response: 400 Bad Request** ❌ (Usuário/Livro não existe)
```json
{
  "sucesso": false,
  "conteudo": 0,
  "mensagemErro": "Usuário ou Livro não encontrado."
}
```

**Response: 401 Unauthorized** ❌
```json
{
  "sucesso": false,
  "conteudo": null,
  "mensagemErro": "Unauthorized"
}
```

**Validações**:
- ✅ Usuário deve existir (idUsuario > 0)
- ✅ Livro deve existir (idLivro > 0)
- ✅ Livro deve estar disponível (não emprestado)
- ✅ Usuário não pode ter empréstimo em atraso
- ✅ Data prevista deve ser futura
- ✅ Requer autenticação JWT Bearer

**Informações**
- Valor padrão: R$ 5,00
- Retorna ID do empréstimo (conteudo)
- Marca livro como indisponível automaticamente

**Status HTTP**:
- `200` → Empréstimo registrado com sucesso
- `400` → Validação falhou (livro ocupado, usuário com atraso, etc)
- `401` → Token ausente ou inválido

#### Devolver Empréstimo
```http
POST /Emprestimo/Devolver
Authorization: Bearer {token}
Content-Type: application/json

{
  "idEmprestimo": 15,
  "dataDevolucao": "2026-03-23T11:50:43.631Z"
}
```

**Request Headers**:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json
```

**Response: 200 OK** ✅ (Com multa)
```json
{
  "sucesso": true,
  "conteudo": "Empréstimo devolvido com sucesso. Multa: R$50,00, Total a pagar: R$50,00",
  "mensagemErro": null
}
```

**Response: 200 OK** ✅ (Sem multa)
```json
{
  "sucesso": true,
  "conteudo": "Empréstimo devolvido com sucesso. Multa: R$0,00, Total a pagar: R$5,00",
  "mensagemErro": null
}
```

**Response: 400 Bad Request** ❌
```json
{
  "sucesso": false,
  "conteudo": 0,
  "mensagemErro": "Empréstimo não encontrado."
}
```

**Response: 401 Unauthorized** ❌
```json
{
  "sucesso": false,
  "conteudo": null,
  "mensagemErro": "Unauthorized"
}
```

**Cálculo de Multa Escalonada**:
- **Sem atraso** (data_devolucao ≤ data_prevista): Multa = R$ 0,00
- **1-3 dias de atraso**: R$ 2,00 × dias
- **4+ dias de atraso**: (3 × R$ 2,00) + (dias_restantes × R$ 3,50)
- **Limite máximo**: R$ 50,00

**Exemplos**:
- Devolução no prazo: Multa = R$ 0,00
- 2 dias atrasado: (2 × R$ 2,00) = R$ 4,00
- 5 dias atrasado: (3 × R$ 2,00) + (2 × R$ 3,50) = R$ 13,00
- 20 dias atrasado: R$ 50,00 (limitado ao máximo)

**Características**:
- ✅ Calcula multa automática baseado em dias de atraso
- ✅ Marca livro como disponível novamente
- ✅ Atualiza status do empréstimo
- ✅ Requer autenticação JWT Bearer
- ✅ Campo `conteudo` contém mensagem descriptiva com valores

**Status HTTP**:
- `200` → Devolução processada com sucesso (com ou sem multa)
- `400` → Empréstimo não encontrado ou dados inválidos
- `401` → Token ausente ou inválido

---

## 🏗️ Arquitetura do Projeto

### Padrão: **Arquitetura em Camadas com Use Cases**

```
BibliotecaApi/
│
├── 📁 Application/ - Camada de Apresentação
│   └── Api/
│       ├── Controllers/ - MVC Controllers (endpoints)
│       │   ├── AuthController.cs
│       │   ├── UsuarioController.cs
│       │   ├── LivroController.cs
│       │   └── EmprestimoController.cs
│       └── Responses/
│           └── ApiResponse<T> - Wrapper genérico de respostas
│
├── 📁 Domain/ - Camada de Lógica de Negócio
│   └── Entities/ - Modelos de domínio com validações
│       ├── UsuarioEntity.cs
│       ├── LivroEntity.cs
│       └── EmprestimoEntity.cs
│
├── 📁 Infrastructure/ - Camada de Infraestrutura
│   ├── Data/
│   │   ├── DbSession.cs - Gerenciador de conexão SQLite
│   │   └── ConfigurationHelper.cs
│   ├── Repositories/ - Acesso a dados com Dapper
│   │   ├── UsuarioRepository.cs
│   │   ├── LivroRepository.cs
│   │   └── EmprestimoRepository.cs
│   ├── Services/
│   │   ├── AuthService.cs - Orquestração de autenticação
│   │   └── JwtService.cs - Geração e validação de JWT
│   ├── Middleware/
│   │   └── AuthenticationMiddleware.cs - Validação de tokens
│   └── IOC/
│       └── DependencyInjection.cs - Configuração de DI
│
└── 📁 UseCases/ - Camada de Casos de Uso
    ├── Usuario/
    │   ├── CadastrarUsuarioUC.cs
    │   └── DTO/CadastrarUsuarioInputDTO.cs
    ├── Livro/
    │   ├── CadastrarLivroUC.cs
    │   ├── ListarLivrosUC.cs
    │   └── DTO/
    │       ├── CadastrarLivroInputDTO.cs
    │       └── ListarLivrosOutputDTO.cs
    └── Emprestimo/
        ├── CadastrarEmprestimoUC.cs
        ├── DevolverEmprestimoUC.cs
        └── DTO/
            ├── CadastrarEmprestimoInputDTO.cs
            └── DevolverEmprestimoInputDTO.cs
```

---

## 🔐 Segurança & Autenticação

### Estratégia de Autenticação

A API utiliza **JWT Bearer Token** com autenticação baseada em **credenciais (seed)**.

#### 1️⃣ Autenticação com Credenciais (Seed)

**Credenciais Padrão**:
- **Email**: `admin@teste.com`
- **Senha**: `bibliotecaapiteste`

Essas credenciais são **validadas em memória** no `AuthService`, **sem armazenamento em banco de dados**.

```csharp
// AuthService.cs
private const string ADMIN_EMAIL = "admin@teste.com";
private const string ADMIN_SENHA = "bibliotecaapiteste";
private const int ADMIN_ID = 1;
private const string ADMIN_NOME = "admin";
```

#### 4️⃣ Validação do Token

- **Assinatura**: HMAC SHA-256 (verificada via Secret Key)
- **Issuer**: `BibliotecaApi`
- **Audience**: `BibliotecaApiUsers`
- **Expiração**: 60 minutos (verificado o claim `exp`)

### Configuração JWT (appsettings.json)
```json
"JwtSettings": {
  "Secret": "essa_chave_super_segura_com_minimo_32_caracteres_para_produca0",
  "Issuer": "BibliotecaApi",
  "Audience": "BibliotecaApiUsers",
  "ExpirationMinutes": 60
}
```

---

## 🧪 Testando a API

### Via Swagger
1. Abra `http://localhost:5023/swagger`
2. Primeiramente, gere um token:
   - Clique em **POST /Auth/generate-token**
   - Em **Try it out**, preencha:
     ```json
     {
       "email": "admin@teste.com",
       "senha": "bibliotecaapiteste"
     }
     ```
   - Clique em **Execute**
   - Copie o token retornado (campo `conteudo`)
3. Clique no botão **Authorize** (cadeado no topo)
4. Cole o token assim: `Bearer {seu_token}`
5. Agora todos os endpoints protegidos estarão disponíveis

### Via VS Code REST Client
Veja o arquivo `BibliotecaApi.http` para exemplos de requisições prontas para testar com a extensão REST Client.

---

## 📋 Requisitos Funcionais Implementados

- ✅ Cadastro de usuários com validação de CPF
- ✅ Cadastro de livros com validação de ISBN (13 dígitos)
- ✅ Listagem de livros (pública)
- ✅ Registro de empréstimos com validação de disponibilidade
- ✅ Validação: Usuário com atraso não pode emprestar
- ✅ Devolução de empréstimos com cálculo de multa escalonada
- ✅ Autenticação JWT Bearer Token
- ✅ Documentação Swagger/OpenAPI
- ✅ Arquitetura em camadas (clean architecture)

---

## 📝 Notas Importantes

1. **Token JWT**: O token expira em 60 minutos. Após expirar, será necessário gerar um novo token.

2. **Rotas Públicas**: 
   - `POST /Auth/generate-token`
   - `GET /Livro/Listar`
   - `/swagger`

3. **Multa por Atraso**: É calculada automaticamente durante a devolução:
   - Primeiros 3 dias: R$ 2,00/dia
   - Próximos dias: R$ 3,50/dia
   - Máximo: R$ 50,00
   - No prazo: R$ 0,00

---

**Desenvolvido como parte de um teste técnico. Autor: Lucas Belluzi**
