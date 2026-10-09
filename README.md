README
Descripción del Proyecto

Este proyecto de Unity (versión 6000.3.19f1) implementa mecánicas esenciales para un juego en primera persona, incluyendo movimiento de personaje, control de cámara, gestión de objetos interactivos tomando el objeto y pudiendo soltarlo, generación automática de proyectiles u obstáculos, y un sistema de potenciadores de velocidad.

Controles del Juego

Movimiento: Teclas W, A, S, D para caminar/moverse.
Salto: Barra espaciadora.
Vista: Movimiento del ratón para mirar alrededor.
Tomar y soltar Objeto: Con la tecla E se agarra el iten y se suelta con r.

Mecánicas y Scripts Principales

1- Movimiento del Jugador
Controlador: Utiliza un componente CharacterController para gestionar las colisiones y el desplazamiento fluido del usuario.   Gravedad y Saltos: Permite saltos múltiples aplicando una fuerza vertical acumulativa y gravedad constante. 

2. Control de Cámara
vista en Primera Persona: Rota la cámara verticalmente limitando el ángulo mediante Mathf.Clamp y rota el cuerpo del jugador horizontalmente.
Cursor: Bloquea y oculta automáticamente el cursor en el centro de la pantalla al iniciar la ejecución.

3. Tomar y Soltar Objetos
Detección Dual: Permite interactuar con objetos etiquetados como "objeto" o "Objeto" apuntándoles con la cámara o por cercanía dentro de una distancia máxima configurable.
Físicas y Jerarquía: Desactiva temporalmente las físicas y el colisionador del objeto tomado para evitar interferencias con el jugador, restaurando su estado original al soltarlo.

4. Generación de Obstáculos
Instanciación Periódica: Utiliza InvokeRepeating para generar prefabs de piedras u objetos de forma automatizada tras un tiempo de retraso inicial y a intervalos regulares.
Impulso y Destrucción: Aplica una velocidad lineal inicial utilizando rbPiedra.linearVelocity y destruye el objeto automáticamente transcurrido un tiempo de vida.

5. Potenciadores de Velocidad
Activación por Disparador (OnTriggerEnter): Detecta cuando el jugador entra en contacto con el objeto coleccionable[cite: 3].
Corrutina de Efecto Temporal: Aumenta la velocidad de movimiento del jugador multiplicándola por un factor específico durante los segundos configurados en duracion, restaurando el valor original al finalizar y destruyendo el power-up.