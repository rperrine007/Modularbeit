# PlantGenius.GUI
## Beschreibung
Wer kennt das nicht? Deine Pflanze ist entweder vertrocknet oder steht im Wasser.
PlantGenius bietet dir die perfekte Unterstützung für die Bewässerung deiner Pflanzen. 
Vertraue auf diese App und gewährleiste die ideale Wasserintervall für jede Pflanze. Mit einer übersichtlichen To-Do-Liste behältst du leicht im Blick, welche deiner Pflanzen wann gegossen werden müssen.

## Projekte
### PlantGenius.DAL
Besteht im essenziellen aus zwei Teilen mit unterschiedlichen Klassen:
- Ordner Models: Enthält die Klassen Pflanzen und Räume
- AppDBContext & DataAccessLayer: Verbindung zu DB wird hergestellt und Methoden zur Interaktion mit der DB erstellt.

PlantGenius.GUI-Projekt und PlantGeniusUserApp.GUI-Projekt können auf alle Klassen in diesem Projekt zugreifen. 

### DataAccessLayerNUnitTests und ModelsNUnitTest
NUnit Tests der Klassen DataAccessLayers, Rooms und Plants.

### PlantGenius.GUI
WPF Applikation zur Verwaltung der Räume. Änderungen werden in der DB gespeichert. 

Es sind zwei verschiedene Ansichten verfügbar: 
- Home: Willkommens-Bildschirm.
- Räume: Auflistung der Räume und die Möglichkeit diese zu verwalten.

### PlantGeniusUserApp.GUI
.NET MAUI Applikation mit welcher eine bearbeitbare Übersicht der gespeicherten Pflanzen in der DB gegeben wird. Die Daten können bearbeitet werden wobei die Änderungen in der DB abgespeichert wird.
Es wird angegeben, wann welche Pflanze gegossen werden muss. 

Es sind vier verschiedene Ansichten vorhanden: 
- Home: Willkommens-Bildschirm.
- Pflanzen: Auflistung wichtigsten Daten der Pflanzen.
- Räume: Auflistung der wichtigsten Daten der Pflanzen, die Pflanzen sind in die zugewiesenen Räume gruppiert.
- Einstellungen: Auflistung möglicher Erweiterungen; noch keine Logik.

... und zwei Unterseiten:
- Pflanzen hinzufügen: Maske um neue Pflanzen der DB hinzuzufügen.
- Pflanzen editieren: Maske um Pflanzen-Daten zu bearbeiten.

## Genutzte Frameworks und Aufbau
Für die Kommunikation mit der genutzten Maria DB wurde das Entity Frame Work (EF) verwendet. Der Code wurde nach dem Model-View-ViewModel (MVVM) Framework gegliedert. Hierzu wurde das Paket CommunityToolkit.Mvvm verwendet, da dieses einige nützliche Funktionen bereitstellt und das Interface INotifyPropertyChanged bereits eingebunden ist.

## Detaillierter Inhalt der NUnit-Tests
Es werden alle Methoden der Klasse DataAccessLayer.cs und Funktionen mit Logik der Klassen Room.cs und Plant.cs getestet. Alle Tests werden im entsprechenden Projekt einer separaten Klasse und auf einer InMemoryDataBase ausgeführt sodass die eigentliche DB nicht durch die Tests belastet oder verändert wird.

Da die Tests alle erfolgreich waren wird an dieser Stelle nicht näher darauf eingegangen.

## Start der Applikation
Die Applikation wurde mittels Visual Studio 2022 programmiert und ist in dieser Programmierumgebung ausführbar. Achte hierbei darauf dass nicht nur die .NET-Desktopentwicklung sondern auch die .NET Multi-Platform App UI-Entwicklung installiert ist. Um die .NET Maui Applikation starten zu können wird ein Android Emulator benötigt. Für iOS und Windows-Machine wurde die Darstellung nicht geprüft, daher wird ein Android Emulator empfohlen. Ausserdem müssen die notwendigen NuGet-Packages installiert werden, siehe hierzu die entsprechenden Projektfiles.
