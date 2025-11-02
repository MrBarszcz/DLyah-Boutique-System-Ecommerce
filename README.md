# DLyah Boutique System Ecommerce

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-9.0-blueviolet)](https://dotnet.microsoft.com/) [![Docker](https://img.shields.io/badge/Docker-blue)](https://www.docker.com/) [![Nginx](https://img.shields.io/badge/Nginx-green)](https://www.nginx.com/) [![MS_SQL_Server](https://img.shields.io/badge/MS_SQL_Server-red)](https://www.microsoft.com/sql-server)

O **DLyah Boutique System** é uma plataforma de e-commerce "full-stack" completa, "pronta para produção", projetada para uma boutique de moda moderna.

Esta plataforma não é apenas um site, mas um sistema de gerenciamento de negócios que permite aos administradores controlar todo o ciclo de vida do produto: desde o cadastro de um item com variações complexas de cor (com swatches em hexadecimal) e estoque, até a promoção visual desse produto em um banner na página principal.

## ✨ Pilares Centrais do Projeto

1.  **Catálogo de Produtos Dinâmico:** Um sistema de cadastro de produtos altamente flexível, permitindo a criação de SKUs complexos baseados em atributos (Cor, Tamanho, Material) e seus valores (ex: Vermelho com Hex `#FF0000`).
2.  **Merchandising Visual Desacoplado:** Um módulo de gerenciamento de conteúdo (banners) que permite ao administrador controlar o layout e as promoções de *qualquer página* do site (Home, Categorias) de forma granular e baseada em regras, lendo a lógica de um `page_slots.json`.
3.  **Infraestrutura Containerizada de Produção:** A aplicação inteira é orquestrada com Docker Compose, usando um proxy reverso Nginx para servir arquivos estáticos e direcionar o tráfego para a aplicação .NET, garantindo alto desempenho e isolamento.

## 💻 Pilha Tecnológica

| Categoria | Tecnologia | Propósito |
| :--- | :--- | :--- |
| **Backend** | **ASP.NET Core MVC (.NET 9)** | Lógica de negócios principal, renderização de HTML. |
| **Banco de Dados** | **MS SQL Server** | Armazenamento de produtos, pedidos, banners, etc. |
| **ORM** | **Entity Framework Core** | Padrão "Code-First" para acesso a dados e migrações. |
| **Web Server (Proxy)** | **Nginx** | Proxy Reverso, Servidor de Arquivos Estáticos (CSS/JS/Imagens). |
| **App Server** | **Kestrel** | O servidor .NET que executa a lógica do C#. |
| **Containerização** | **Docker & Docker Compose** | Orquestração de todos os serviços (App, DB, Nginx). |
| **Padrões** | **MVC, Repository, Service** | Separação de responsabilidades (SoC). |

---

## 📐 Arquitetura da Infraestrutura (Docker)

O projeto é executado como uma pilha de três containers que se comunicam através de uma rede Docker privada (`dlyah-net`), com um volume persistente para o banco de dados e um volume compartilhado para arquivos estáticos.

Esta arquitetura desacopla a entrega de arquivos estáticos (Nginx) da lógica de negócios (App C#), melhorando drasticdamente o desempenho.

```mermaid
graph TD
    subgraph "Navegador do Usuário"
        U(Usuário)
    end

    U -- "GET / (Página HTML)" --> Nginx[<img src='[https://static-00.iconduck.com/assets.00/nginx-icon-2048x2048-g2i5cpun.png](https://static-00.iconduck.com/assets.00/nginx-icon-2048x2048-g2i5cpun.png)' width='40' /><br/>Nginx Proxy<br/>(nginx)]
    U -- "GET /logo.png (Imagem)" --> Nginx

    subgraph "Rede Docker (dlyah-net)"
        Nginx -- "proxy_pass (HTML)" --> App[<img src='[https://upload.wikimedia.org/wikipedia/commons/thumb/e/ee/.NET_Core_Logo.svg/2048px-.NET_Core_Logo.svg.png](https://upload.wikimedia.org/wikipedia/commons/thumb/e/ee/.NET_Core_Logo.svg/2048px-.NET_Core_Logo.svg.png)' width='40' /><br/>App C# / Kestrel<br/>(app)]
        
        App -- "SELECT/INSERT" --> DB[<img src='[https://symbols.getvecta.com/stencil_261/33_sql-database.00d1d60b37.svg](https://symbols.getvecta.com/stencil_261/33_sql-database.00d1d60b37.svg)' width='40' /><br/>SQL Server<br/>(db)]
        
        App -- "POST (Escreve Imagens)" --> VolStatic(vol: static-files<br/>/app/wwwroot)
        Nginx -- "GET (Lê Imagens)" --> VolStatic
        
        DB -- "Persiste Dados" --> VolDB(vol: db-dlyah-data<br/>/var/opt/mssql)
    end
```

## 📂 Arquitetura da Aplicação (Domínios de Negócio)

O sistema é dividido em três domínios de negócio principais que se interconectam.

### 1. Domínio: Catálogo de Produtos

O núcleo do e-commerce, focado em flexibilidade.
* **Product (Produto Base):** A entidade "pai" (ex: "Vestido Longo").
* **Attribute (Atributo):** Os tipos de variação (ex: "Cor", "Tamanho").
* **AttributeValue (Valor do Atributo):** O valor específico, que armazena dados ricos (ex: `Value: "Vermelho"`, `HexCode: "#FF0000"`).
* **ProductVariant (SKU):** A unidade vendável que possui **Preço** e **Estoque**. É a combinação de um `Product` e vários `AttributeValue`.

### 2. Domínio: Merchandising Visual (Banners)

O módulo de gerenciamento de conteúdo que permite ao administrador controlar o layout.
* **BannerModel:** A unidade de conteúdo (imagem, título, link).
* **`page_slots.json` (Configuração):** O "cérebro" do layout. Um arquivo JSON externo que define as posições (ex: `MainCarousel`) disponíveis em cada tipo de página (ex: `PageType: "Home"`).
* **BannerPlacement:** A entidade que conecta um `BannerModel` a uma `Position` em uma `PageName`.

### 3. Domínio: Vendas e Usuários

O fluxo transacional padrão de um e-commerce.
* **Customer / User:** Contas de clientes.
* **Cart (Carrinho):** Armazena os `ProductVariantId` selecionados.
* **Order / OrderItem:** O registro final da compra, armazenando o preço no momento da transação.

## 🔄 Fluxos de Lógica Fundamentais

Estes fluxos demonstram como os domínios e a infraestrutura colaboram.

### Fluxo 1: Upload de Imagem de Produto (Admin)
*Este fluxo mostra como o Nginx, o App C# e o Volume compartilhado trabalham juntos.*

1.  **Formulário:** O Admin envia um formulário `POST` para `/Admin/Product/Create` com dados e um arquivo de imagem.
2.  **Nginx (Proxy):** O Nginx recebe o `POST`. Como a URL não termina em `.jpg` ou `.css`, ele aciona a regra `location /` e repassa (`proxy_pass`) a requisição inteira para o container `app`.
3.  **C# (App):** O `ProdutoController` recebe a requisição e o `IFormFile`.
4.  **C# (Escrita em Volume):** O `FileUploadService` salva a imagem fisicamente em `/app/wwwroot/images/produtos/imagem.jpg`.
5.  **Docker (Volume):** Como `/app/wwwroot` está montado no volume `static-files`, a imagem agora existe no "disco compartilhado".
6.  **C# (Banco de Dados):** O C# salva o caminho (ex: `/images/produtos/imagem.jpg`) na tabela `Produtos` do `db`.

### Fluxo 2: Entrega de Imagem (Cliente)
*Este fluxo demonstra a otimização de performance do Nginx.*

1.  **HTML:** O cliente carrega uma página que contém a tag `<img src="/images/produtos/imagem.jpg">`.
2.  **Navegador:** O navegador faz uma nova requisição `GET` para `/images/produtos/imagem.jpg`.
3.  **Nginx (Servidor Estático):** O Nginx intercepta a requisição. Ele vê que ela termina em `.jpg` e aciona a regra `location ~ \.(jpg)$`.
4.  **Nginx (Leitura de Volume):** Em vez de incomodar o C#, o Nginx vai direto ao "disco compartilhado" (montado em `/var/www/static`), pega a imagem e a entrega ao cliente.
5.  **Resultado:** A imagem é entregue muito mais rápido, e o app C# (`Kestrel`) fica livre para processar lógica de negócios.

### Fluxo 3: Jornada do Cliente (Interação dos Domínios)
1.  **Home:** Cliente acessa `/`. O `HomeController` busca `BannerPlacement` para `PageName: "Home"`.
2.  **Navegação:** Cliente clica em um banner com `LinkUrl: "/product/vestido-floral"`.
3.  **Página do Produto:** O `ProductController` busca o `Product` e todas as suas `ProductVariants` e `AttributeValues`.
4.  **Frontend (UI):** O JavaScript na View usa os `AttributeValues` para renderizar os *swatches* de cor (círculos coloridos) usando os dados do `HexCode`.
5.  **Seleção:** O cliente seleciona "Rosa" e "M". O JS identifica o `ProductVariantId` correto.
6.  **Carrinho:** A `ProductVariantId` é enviada via `POST` para o `CartController/Add`.
7.  **Checkout:** Um `Order` é criado a partir dos itens do `Cart`, salvando um `OrderItem` com o preço daquele momento.

## 🗺️ Próximos Passos (Roadmap)

Com a base do Docker Compose pronta, os próximos passos lógicos para este projeto são:

1.  **CI/CD com Jenkins:** Configurar um servidor Jenkins (que pode ser outro container Docker) para monitorar o repositório Git. Em cada `git push`, o Jenkins deve automaticamente:
    * Rodar os testes (`dotnet test`).
    * Construir e publicar a nova imagem Docker do `app`.
    * Fazer deploy da nova imagem no servidor de produção (ex: via SSH e rodando `docker compose up -d --pull`).
2.  **Orquestração com Kubernetes (K8s):** Para produção em larga escala, migrar esta configuração do `docker-compose.yml` para manifestos do Kubernetes, permitindo auto-scaling, balanceamento de carga e zero downtime em deploys.