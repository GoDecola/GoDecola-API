# GoDecola API

**GoDecola API** é uma aplicação RESTful desenvolvida como parte do projeto final da formação FullStack Avanade DecolaTech VI. A plataforma simula um sistema de agência de viagens, incluindo cadastro de usuários, reservas de pacotes, avaliações, wishlist, e administração dos dados.


## Sumário

- [Diagrama Entidade-Relacionamento (ER)](#diagrama-entidade-relacionamento-er)
- [Arquitetura do Sistema](#arquitetura-do-sistema)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Instruções de Uso](#instruções-de-uso)
- [Contribuidores](#contribuidores)
- [Licença](#licença)


## Diagrama Entidade Relacionamento ER

```mermaid
---
config:
  theme: redux-color
---
erDiagram
  User {
    int id_usuario PK
    string FirstName
    string LastName
    string Email
    string Username
    string Phone
    string Password
    string CPF
    string RNE
    string Passport
    DateTime CreatedAt
  }
  TravelPackage {
    int Id PK
    string Title
    string Description
    long Price
    string Destination
    datatime StartDate
    datatime EndDate
    bool IsActive
    int NumberGuests
    double DiscountPercentage
    datetime PromotionStartDate
    datetime PromotionEndDate
  }
  TravelPackageMedia {
    int Id PK
    int TravelPackageId FK
    string FilePath
    string MimeType
    datetime UploadDate
  }
  Address{
    int Id
    string AddressLine1
    string AddressLine2
    string ZipCode
    string Country
    string State
    string City
    string Neighborhood
    string Latitude
    string Longitude
  }
  AccommodationDetails {
    int Id PK
    int TravelPackageId FK
    int AddressId
    int NumberBaths
    int NumberBeds
    bool HasWifi
    bool HasParking
    bool HasPool
    bool HasGym
    bool HasRestaurant
    bool HasPetFriendly
    bool HasAirConditioning
    bool HasBreakfastIncluded
  }
  Reservation {
    int Id PK
    int UserId FK
    int TravelPackageId FK
    datatime CheckInDate
    datatime CheckOutDate
    long TotalPrice
    string Status
    datetime ReservationDate
  }
  Guests {
    int Id PK
    int ReservationId FK
    string FirstName
    string LastName
    string Email
    string Phone
    string CPF
    string RNE
    string Passport
    date DateOfBirth
  }
  Payment {
    int Id PK
    string StripePaymentIntentId FK
    int ReservationId FK
    datetime PaymentDate
    string Method
    string Status
    long AmountPaid
    string RedirectUrl
    string UrlVoucher
  }
  StripeSettings{
    string PublishableKey
    string SecretKey
  }
  Review {
    int Id PK
    int UserId FK
    int TravelPackageId FK
    double Rating
    string Comment
    datetime ReviewDate
  }
  Wishlist {
    int Id PK
    int UserId FK
    int TravelPackageId FK
    datetime AddedDate
  }

  User ||--o{ Reservation : makes
  User ||--o{ Review : writes
  User ||--o{ Wishlist : owns

  TravelPackage ||--o{ Reservation : includes
  TravelPackage ||--|| AccommodationDetails : has
  TravelPackage ||--o{ Review : receives
  TravelPackage ||--o{ TravelPackageMedia : contains
  TravelPackage ||--o{ Wishlist : appearsIn

  Reservation ||--o{ Guests : has
  Reservation ||--|| Payment : paidBy

  Payment ||--|| StripeSettings : has

  AccommodationDetails ||--|| Address : locatedAt
```

> Diagrama de classes do projeto GoDecola API

## Arquitetura do Sistema

```mermaid
---
config:
  theme: base
  layout: elk
---
graph LR

subgraph Cliente
    A[Browser]
end

subgraph Frontend
    B[Frontend - React]
end

subgraph Backend
    C[API REST - ASP.NET Core]
    D[Autenticação e Autorização - ASP.NET Identity + JWT] 
    E[Camada de Serviços - Regras de Negócio]
    F[Repositórios - EF Core]
    K[AutoMapper - Mapeamento DTOs]
end

subgraph Banco_de_Dados
    G[SQL Server]
end

subgraph Serviços_Externos
    H[Gateway de Pagamento -  Stripe]
    I[Serviço de Email - SMTP]
end

A --> B
B --> C
C --> D
C --> E
E --> F
E --> K
F --> G
C --> H
C --> I
```

## Tecnologias Utilizadas

- C# (.NET 9)
- ASP.NET Core Web API
- SQL Server
- Integração de pagamento Stripe
- Git para versionamento
- Swagger

## Instruções de uso

#### Clonar o repositório
``` bash
git clone https://github.com/GoDecola/GoDecola-API.git 
```
#### Acessar o diretório
``` bash
cd GoDecola-API
```
#### Executar o projeto
```bash
dotnet run
```
> 💡 Configure as chaves no **appsettings.json** para conexão com o banco de dados e API de pagamento.

## Licença
Este projeto está licenciado sob a [Licença MIT](LICENSE).
