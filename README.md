# Vavatech.201901.WebApiSecurity
- X-Frame-Options: https://niebezpiecznik.pl/post/x-frame-options-zacznij-stosowac/
- https://sekurak.pl/wprowadzenie-do-narzedzia-zed-attack-proxy-zap/


## ngrok
W przypadku gdy chcemy wystawić stronę w IIS Express należy ngrok uruchomić w następujący sposób:
~~~ bash
ngrok http -host-header="localhost:[port]" [port]
~~~
