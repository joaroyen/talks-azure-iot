# Rev 1

Denne utgaven fungerte ikke da TIP 120 kan kun switcje opp til base spenning. Løsning var å enre til å switche til GND ved å bodge en av skruterminalene til GND og bytte om hvor Load kobles til. 

# Rev 2

- TIP120 switcher GND og ikke 12V
- Så kort som mulig avstand til "høy spenning"
- Pull-down til IO pin for å forhindre at transistoren skrur på LED når ESP8266 får strøm / bootes
- Marker positiv skruterminal med spenning
- Korrekt footprint for motstandene
- Monteringshull

# Rev 3

- Byttet om 12V og Load terminaler
- Flyttet R1 og R2 litt lengre ned
