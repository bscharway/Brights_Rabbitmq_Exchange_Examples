# Microservices with RabbitMQ - Example Project

## Projektoversigt
Dette projekt består af flere mikroservices, der bruger RabbitMQ som beskedbro (message broker) til at håndtere kommunikation mellem tjenesterne. Hvert mikroservice simulerer forskellige RabbitMQ exchange-typer, som giver en omfattende forståelse af, hvordan beskeder kan rutes mellem services.

De følgende RabbitMQ exchange-typer dækkes:
- **Fanout Exchange**: Disse mikroservices illustrerer brugen af en fanout exchange, hvor beskeder sendes til alle køer bundet til exchange'en.
- **Direct Exchange**: Producer- og forbrugermikroservices, der demonstrerer direkte beskedlevering til specifikke køer baseret på routing keys.
- **Headers Exchange**: Et eksempel på, hvordan man bruger headers til at rute beskeder afhængigt af specifikke attributter.
- **Topic Exchange**: Viser, hvordan beskeder rutes dynamisk baseret på routing keys og mønstre.

RabbitMQ bruges som en central message broker til at styre beskedudveksling mellem disse mikroservices.

## Struktur
Mikroservices inkluderet i dette projekt:
- **Fanout Exchange Producer og Consumer Mikroservices**: Produceren sender beskeder til en fanout exchange, og forbrugerne modtager beskeder fra dedikerede køer (`queue_for_consumer_1`, `queue_for_consumer_2`, `queue_for_consumer_3`).
- **Direct Exchange Producer og Consumer Mikroservices**: Produceren sender beskeder til bestemte køer baseret på routing keys (fx "error"), og forbrugerne modtager kun relevante beskeder.
- **Headers Exchange Producer og Consumer Mikroservices**: Produceren specificerer headers, som bruges til at rute beskeder, og forbrugerne modtager beskeder baseret på disse headers.
- **Topic Exchange Producer og Consumer Mikroservices**: Produceren bruger routing keys med mønstre til at bestemme, hvilke køer beskederne skal leveres til.

## Sådan Kører Du Projektet
For at starte alle mikroservices og RabbitMQ, skal du bruge Docker og Docker Compose. Følg disse trin for at komme i gang:

1. Sørg for, at Docker og Docker Compose er installeret på din maskine.
2. Klon dette repository til din lokale maskine.
3. Naviger til roden af projektet, hvor `docker-compose.yml`-filen er placeret.
4. Kør følgende kommando i terminalen:

   ```sh
   docker-compose up --build
   ```

   Denne kommando vil bygge alle mikroservices og starte RabbitMQ sammen med de nødvendige containerinstanser.

## RabbitMQ Management Interface
Efter at have startet containerne, kan RabbitMQ Management UI tilgås i din webbrowser ved at gå til:

- [http://localhost:15672](http://localhost:15672)

Brug følgende loginoplysninger for at logge ind:
- **Brugernavn**: `guest`
- **Adgangskode**: `guest`

Her kan du få et overblik over køer, exchanges, og de beskeder, der flyder gennem systemet.

## Fejlfinding
- **Kan ikke forbinde til RabbitMQ**: Sørg for, at RabbitMQ-containeren er startet korrekt, og at portene `5672` og `15672` ikke er blokeret eller i konflikt med andre tjenester.
- **Mikroservice Crasher**: Sørg for, at alle nødvendige afhængigheder er installeret, og at dine Dockerfiles er korrekt defineret.

## Afslutning
Dette projekt giver en god forståelse af, hvordan RabbitMQ kan bruges til at orkestrere kommunikation mellem mikroservices ved hjælp af forskellige exchange-typer. Med `docker-compose` kan alle services let køres og testes lokalt.

Hvis du har yderligere spørgsmål eller problemer, er du velkommen til at kontakte os eller oprette en issue i GitHub-repositoriet.

