# PlantGenius.GUI
## Beschreibung
Wer kennt das nicht? Deine Pflanze ist entweder vertrocknet oder steht im Wasser.
PlantGenius bietet dir die perfekte Unterstützung für die Bewässerung deiner Pflanzen. 
Vertraue auf diese App und gewährleiste die ideale Wassermenge für jede Pflanze. Mit einer übersichtlichen To-Do-Liste behältst du leicht im Blick, welche deiner Pflanzen wann gegossen werden müssen.


## Projekte
### PlantGenius.DAL
Besteht im essenziellen aus zwei Teilen mit unterschiedlichen Klassen:
- Ordner Models: Enthält die Klassen Pflanzen und Räume
- AppDBContext & DataAccessLayer: Verbindung zu DB wird hergestellt und Methoden zur Interaktion mit der DB erstellt.

PlantGenius.GUI-Projekt und PlantGeniusUserApp.GUI-Projekt können auf alle Klassen in diesem Projekt zugreifen. 

### DataAccessLayerNUnitTests
NUnit Tests der Klassen DataAccessLayers, Rooms und Plants.

### PlantGenius.GUI
WPF Applikation zur Verwaltung der Räume. Änderungen werden in der DB gespeichert. 

### PlantGeniusUserApp.GUI
.NET MAUI Applikation mit welcher eine bearbeitbare Übersicht der gespeicherten Pflanzen in der DB gegeben wird. Die Daten können bearbeitet werden wobei die Änderungen in der DB abgespeichert wird.
Es wird angegeben, wann welche Pflanze gegossen werden muss. 

Es sind vier verschiedene Ansichten vorhanden: 
- Home: Willkommens-Bildschirm.
- Pflanzen: Auflistung wichtigsten Daten der Pflanzen.
- Räume: Auflistung der wichtigsten Daten der Pflanzen, die Pflanzen sind in die zugewiesenen Räume gruppiert.
- Optionen: Noch nichts, wäre eine Erweiterungsmöglichkeit.
- Pflanzen hinzufügen: Maske um neue Pflanzen der DB hinzuzufügen.
- Pflanzen editieren: Maske um Pflanzen-Daten zu bearbeiten.

## Genutzte Frameworks und Aufbau
Für die Kommunikation mit der genutzten Maria DB wurde das Entity Frame Work (EFF) verwendet. Der Code wurde nach dem Model-View-ViewModel (MVVM) Framework gegliedert. Ausserdem wurde das Paket CommunityToolkit.Mvvm verwendet, da dieses einige nützliche Funktionen bereitstellt und das wichtige Interface INotifyPropertyChanged bereits eingebunden ist.

## Detaillierter Inhalt der NUnit-Tests
### Berschreibung der Tests
In diesem Projekt werden folgende Methoden der Klasse DataAccessLayer.cs getestet:
- public async Task<List<Room>> GetRooms()
- public async Task AddRoomToDB(...)
- public async Task UpdateRoomToDB(...)
- public async Task DeleteRoomFromDB(...)
- public async Task UpdateRoomSortNumber(...)
- public async Task RefreshSortRooms()   

- public async Task<List<Plant>> LoadPlantsFromDB()
- public async Task AddPlantToDB(...)
- public async Task UpdatePlantsToDB(...)
- public async Task DeletePlantFromDB(...)

- public async Task<HashSet<int>> GetRoomsWithPlants()


Zusätzlich werden Funktionen mit Logik der Klassen Room und Plant in einem separaten Projekt getestet.

Alle Tests werden in einer separaten Klasse und auf einer InMemoryDataBase ausgeführt sodass die eigentliche DB nicht durch die Tests belastet und verändert wird.
Da die Tests alle erfolgreich waren wird an dieser Stelle nicht näher darauf eingegangen.
