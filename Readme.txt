# Accessibility Checker

Questo progetto contiene un semplice servizio Web in .NET 8 che consente di analizzare una URL e verificare alcuni requisiti di accessibilità (WCAG 2.2). 

## Come eseguire

1. Installare .NET 8.
2. Spostarsi nella cartella `AccessibilityChecker` ed eseguire:
   ```bash
   dotnet run
   ```
3. Aprire `http://localhost:5000` nel browser per utilizzare la semplice interfaccia web basata su Bootstrap 5.
4. In alternativa è possibile utilizzare gli endpoint HTTP:
   - `POST /api/accessibility/analyze` con parametro `url` nel body per ottenere un report JSON.
   - `POST /api/accessibility/declaration` con parametro `url` nel body per ricevere un PDF chiamato "DichiarazioneAccessibilita.pdf".

Gli esempi di controllo comprendono titolo della pagina, attributo `lang`, immagini senza `alt` e campi input senza label.
