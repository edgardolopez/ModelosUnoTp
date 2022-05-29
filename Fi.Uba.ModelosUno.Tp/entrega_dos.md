## Situacion
Se trata de un problema del viajante con restricciones a los lugares que puede ir y la carga que debe tomar/dejar en cada lugar.

## Ideas de como lo van a intentar resolver
Primero intento resolver el caso mas simple (Solo busco el proximo mas cerca)
  - Busco el primer Banco que tenga dimero para entregar y luego continuo con los bancos mas cerca posibles de ese en los cuales me permita entregar o retirar dinero

## Objetivo
Determinar que camino realizar para minimizar la distancia reccorrida total, pasando por todos los bancos y 
teniendo en cuenta que el camion no puede tener saldo negativo ni saldo que supere el monto maximo, durante el recorrido en un dia.

## Hipótesis y Supuestos

- Desde la sede central al primer lugar la distancia es depreciable.
- El camion no se rompe, no tiene fallas y no debe cargar gasolina.
- No se pierde dinero.
- Todas las distancias son conocidas.
- El camion empieza con un monto de 0
- La distancia del banco A al banco B es la misma que la del banco B al banco A.

# Modelo Matematico
## Constantes

- $CAPACIDAD \in \mathbb{N}:$ Cuanto dinero puede transportar el camión.

- $CANTIDAD\_BANCOS \in \mathbb{N}:$ Cantidad de bancos a visitar.

- $DISTANCIA_{i, j} \in \mathbb{N}:$ Distancia del banco $i$ al banco $j$.

- $DEMANDA_{i} \in \mathbb{N}:$ Cuanto dinero entrega o recibe el banco $i$.

## Variables

- $BANCOS \in \{1...CANTIDAD\_BANCOS\}$

- $Y_{i, j} \in \{0, 1\}:$ bivalente que indica el recorrido del banco $i$ al banco $j$.

- $U_{i} \in \mathbb{N}:$ Número de secuencia del banco $i$ en el recorrido.

- $P_{i} \in \mathbb{N}_0:$ Cantidad de dinero que tiene el camión al llegar al punto $i$.

Solo se visita una vez el banco: 

$$\sum_{\substack{i = 1\\ j \ne i}}^{BANCOS} Y_{i,j} = 1 \quad \forall j \in BANCOS$$

$$\sum_{\substack{j = 1\\ j \ne i}}^{BANCOS} Y_{i,j} = 1 \quad \forall i \in BANCOS$$

### Elimina subtours

$$U_{i} - U_{j} + CANTIDAD\_BANCOS Y_{i,j} \le CANTIDAD\_BANCOS - 1$$
$$\forall i,j \in BANCOS$$ 

### En todo momento el camión debe tener entre 0 y CAPACIDAD máxima de dinero. 

$$0 \leq P_{i} \leq CAPACIDAD$$
$$\forall i \in BANCOS$$

### Vinculo

$$-M * (1 - Y_{i,j}) + DEMANDA_j \leq P_j - P_i \leq DEMANDA_i + M * (1 - Y_{i,j})$$
$$\forall i,j \in BANCOS$$


## Función Objetivo

$$ \: Z_{MIN} = \mathop{\sum\sum}_{\substack{i = 1 j = 1 \\ i \ne j}}^{BANCOS} Y_{i,j} * DISTANCIA_{i,j}$$
