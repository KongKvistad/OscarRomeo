

# Hvordan kjøre:
- clone prosjektet
- docker-compose up --build

Dette vil gi deg en løsning som laster dataene asynkront. (Dette er enklest, da skal du ikke måtte laste ned SDK.)

Om man vil ha en static build hvor alle stasjonene er tegnet ut som en del av source,
må man ha API-et kjørende før man kompilerer frontend med `npm run build` etterfulgt av `npm run start` i `/frontend`:
![source](https://github.com/KongKvistad/OscarRomeo/assets/42936783/ff8741c4-7807-4ea2-a1e3-c1036fb4c97f).


# avhengigheter: 

porosjektet baserer seg på:
next.js v.14: https://nextjs.org/docs/getting-started/installation
.net 6 SDK: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
react-leaflet: https://react-leaflet.js.org/



MISC:
.env-filer:
Disse er ikke skjult fra git,
da løsningen ikke skal dyttes til prod.
