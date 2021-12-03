<map version="freeplane 1.9.8">
<!--To view this file, download free mind mapping software Freeplane from https://www.freeplane.org -->
<node TEXT="AusBat" FOLDED="false" ID="ID_696401721" CREATED="1610381621824" MODIFIED="1635135547442" STYLE="oval">
<font SIZE="18"/>
<hook NAME="MapStyle">
    <properties fit_to_viewport="false" show_icon_for_attributes="true" show_note_icons="true" edgeColorConfiguration="#808080ff,#ff0000ff,#0000ffff,#00ff00ff,#ff00ffff,#00ffffff,#7c0000ff,#00007cff,#007c00ff,#7c007cff,#007c7cff,#7c7c00ff" associatedTemplateLocation="template:/standard-1.6.mm"/>

<map_styles>
<stylenode LOCALIZED_TEXT="styles.root_node" STYLE="oval" UNIFORM_SHAPE="true" VGAP_QUANTITY="24 pt">
<font SIZE="24"/>
<stylenode LOCALIZED_TEXT="styles.predefined" POSITION="right" STYLE="bubble">
<stylenode LOCALIZED_TEXT="default" ID="ID_271890427" ICON_SIZE="12 pt" COLOR="#000000" STYLE="fork">
<arrowlink SHAPE="CUBIC_CURVE" COLOR="#000000" WIDTH="2" TRANSPARENCY="200" DASH="" FONT_SIZE="9" FONT_FAMILY="SansSerif" DESTINATION="ID_271890427" STARTARROW="DEFAULT" ENDARROW="NONE"/>
<font NAME="SansSerif" SIZE="10" BOLD="false" ITALIC="false"/>
<richcontent CONTENT-TYPE="plain/auto" TYPE="DETAILS"/>
<richcontent TYPE="NOTE" CONTENT-TYPE="plain/auto"/>
</stylenode>
<stylenode LOCALIZED_TEXT="defaultstyle.details"/>
<stylenode LOCALIZED_TEXT="defaultstyle.attributes">
<font SIZE="9"/>
</stylenode>
<stylenode LOCALIZED_TEXT="defaultstyle.note" COLOR="#000000" BACKGROUND_COLOR="#ffffff" TEXT_ALIGN="LEFT"/>
<stylenode LOCALIZED_TEXT="defaultstyle.floating">
<edge STYLE="hide_edge"/>
<cloud COLOR="#f0f0f0" SHAPE="ROUND_RECT"/>
</stylenode>
<stylenode LOCALIZED_TEXT="defaultstyle.selection" BACKGROUND_COLOR="#4e85f8" BORDER_COLOR_LIKE_EDGE="false" BORDER_COLOR="#4e85f8"/>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.user-defined" POSITION="right" STYLE="bubble">
<stylenode LOCALIZED_TEXT="styles.topic" COLOR="#18898b" STYLE="fork">
<font NAME="Liberation Sans" SIZE="10" BOLD="true"/>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.subtopic" COLOR="#cc3300" STYLE="fork">
<font NAME="Liberation Sans" SIZE="10" BOLD="true"/>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.subsubtopic" COLOR="#669900">
<font NAME="Liberation Sans" SIZE="10" BOLD="true"/>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.important" ID="ID_67550811">
<icon BUILTIN="yes"/>
<arrowlink COLOR="#003399" TRANSPARENCY="255" DESTINATION="ID_67550811"/>
</stylenode>
</stylenode>
<stylenode LOCALIZED_TEXT="styles.AutomaticLayout" POSITION="right" STYLE="bubble">
<stylenode LOCALIZED_TEXT="AutomaticLayout.level.root" COLOR="#000000" STYLE="oval" SHAPE_HORIZONTAL_MARGIN="10 pt" SHAPE_VERTICAL_MARGIN="10 pt">
<font SIZE="18"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,1" COLOR="#0033ff">
<font SIZE="16"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,2" COLOR="#00b439">
<font SIZE="14"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,3" COLOR="#990000">
<font SIZE="12"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,4" COLOR="#111111">
<font SIZE="10"/>
</stylenode>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,5"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,6"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,7"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,8"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,9"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,10"/>
<stylenode LOCALIZED_TEXT="AutomaticLayout.level,11"/>
</stylenode>
</stylenode>
</map_styles>
</hook>
<hook NAME="AutomaticEdgeColor" COUNTER="10" RULE="ON_BRANCH_CREATION"/>
<node TEXT="Distribution" POSITION="right" ID="ID_1316568271" CREATED="1635135549330" MODIFIED="1635293773024">
<edge COLOR="#00ff00"/>
<richcontent TYPE="NOTE" CONTENT-TYPE="xml/">
<html>
  <head>
    
  </head>
  <body>
    <p>
      Use Azure App Center
    </p>
    <p>
      ThingsToRemember
    </p>
    <p>
      ...
    </p>
  </body>
</html></richcontent>
<node TEXT="IOS" ID="ID_1409679174" CREATED="1635135585761" MODIFIED="1635318071987"><richcontent TYPE="NOTE" CONTENT-TYPE="xml/">
<html>
  <head>
    
  </head>
  <body>
    <p>
      Create IPA file https://docs.microsoft.com/en-us/xamarin/ios/deploy-test/app-distribution/ipa-support?tabs=windows
    </p>
    <p>
      Set target device [Remote Device] when building. Build [Release]
    </p>
  </body>
</html></richcontent>
</node>
<node TEXT="Android" ID="ID_566895906" CREATED="1635135591881" MODIFIED="1635135690433"><richcontent TYPE="NOTE" CONTENT-TYPE="xml/">
<html>
  <head>
    
  </head>
  <body>
    <p>
      Get signed APK file from BIN in VS
    </p>
  </body>
</html></richcontent>
</node>
<node TEXT="App Center" FOLDED="true" ID="ID_44342366" CREATED="1635320227952" MODIFIED="1635463626530">
<node TEXT="IOS" ID="ID_1459675838" CREATED="1635320237977" MODIFIED="1635463626529" VSHIFT_QUANTITY="-0.75 pt">
<node TEXT="Must get UDID for each tester and add to Provisioning Profile" ID="ID_772292838" CREATED="1635320248232" MODIFIED="1635320281311"/>
<node TEXT="Set Group as public (then no login)" ID="ID_184695448" CREATED="1635320281998" MODIFIED="1635320321090"/>
</node>
</node>
<node TEXT="Versioning" ID="ID_179456736" CREATED="1638506086664" MODIFIED="1638506092294">
<node TEXT="Change data version in data_version.json file" ID="ID_731456790" CREATED="1638506092807" MODIFIED="1638506112617"/>
<node TEXT="Change code version in code_version.json file" ID="ID_328516320" CREATED="1638506116062" MODIFIED="1638506133407"/>
<node TEXT="Change version in device properties. Set it to data_version" ID="ID_1466621401" CREATED="1638506135231" MODIFIED="1638506310617"/>
</node>
</node>
<node TEXT="Upgrading MacBook" POSITION="left" ID="ID_1260274461" CREATED="1635209452459" MODIFIED="1635210278693">
<edge COLOR="#ff00ff"/>
<richcontent TYPE="NOTE" CONTENT-TYPE="xml/">
<html>
  <head>
    
  </head>
  <body>
    <p>
      Use OpenCore to patch OS:
    </p>
    <p>
      https://www.youtube.com/watch?v=znlhI6f7x1Q
    </p>
  </body>
</html></richcontent>
</node>
<node TEXT="IconsAndSplashScreens" POSITION="right" ID="ID_235657070" CREATED="1635293620672" MODIFIED="1635293878744">
<edge COLOR="#00ffff"/>
<node TEXT="Icons" FOLDED="true" ID="ID_1751316084" CREATED="1635293644880" MODIFIED="1635293662785"><richcontent TYPE="NOTE" CONTENT-TYPE="xml/">
<html>
  <head>
    
  </head>
  <body>
    <p>
      https://stackoverflow.com/questions/37945767/how-to-change-application-icon-in-xamarin-forms
    </p>
  </body>
</html></richcontent>
<node TEXT="IOS" ID="ID_915341172" CREATED="1635463456103" MODIFIED="1635463460055">
<node TEXT="Images in Assets Catalog, start with it empty and just add the ones the Build complains are missing. Do not add anything more" ID="ID_229386797" CREATED="1635463460057" MODIFIED="1635463532450"/>
<node TEXT="Android" ID="ID_348784011" CREATED="1635463539903" MODIFIED="1635463545096">
<node TEXT="Image is referenced am MainActivity.cs. Ensure its also referenced in Startup Activity" ID="ID_1705418337" CREATED="1635463545099" MODIFIED="1635463618176"/>
</node>
</node>
</node>
<node TEXT="Splash" ID="ID_1968826518" CREATED="1635463223217" MODIFIED="1635463230969">
<node TEXT="Android" ID="ID_882336192" CREATED="1635463230972" MODIFIED="1635463239025">
<node TEXT="Place image in Resources/drawable folder" ID="ID_1353674177" CREATED="1635463239028" MODIFIED="1635463273745"/>
<node TEXT="There is a Startup application and set of xml and style docs to create splash" ID="ID_25335279" CREATED="1635463275296" MODIFIED="1635463319208"/>
</node>
<node TEXT="IOS" FOLDED="true" ID="ID_1362629066" CREATED="1635463323767" MODIFIED="1635463334927">
<node TEXT="Hack - edit storyboard file, put x1,x2 and x3 images in Resources folder" ID="ID_715381447" CREATED="1635463334929" MODIFIED="1635463444584"/>
<node TEXT="Important - Rename file with each change, otherwise it used cached version" ID="ID_1089092165" CREATED="1635469003855" MODIFIED="1635469028904"/>
</node>
</node>
</node>
<node TEXT="Logins" POSITION="left" ID="ID_1486314238" CREATED="1635378022577" MODIFIED="1635378042440">
<edge COLOR="#007c00"/>
<node TEXT="AppCenter" ID="ID_1456900931" CREATED="1635378043910" MODIFIED="1635378329028"><richcontent TYPE="NOTE" CONTENT-TYPE="xml/">
<html>
  <head>
    
  </head>
  <body>
    <p>
      Login using <b>Google</b>:
    </p>
    <p>
      treeapps.develop@gmail.com
    </p>
    <p>
      Treeappsdevelop@256
    </p>
  </body>
</html></richcontent>
</node>
</node>
<node TEXT="Documentation" POSITION="right" ID="ID_1645926589" CREATED="1635455067530" MODIFIED="1635455073733">
<edge COLOR="#7c007c"/>
<node TEXT="File naming convension" ID="ID_1461854879" CREATED="1635455074247" MODIFIED="1635455167003"><richcontent TYPE="NOTE" CONTENT-TYPE="xml/">
<html>
  <head>
    
  </head>
  <body>
    <p class="MsoPlainText">
      <span lang="EN-AU" style="color: #00B0F0"><font color="#00B0F0">Species name is taken from the EXCEL table: &nbsp;<!--[if gte vml 1]><v:shapetype
 id="_x0000_t75" coordsize="21600,21600" o:spt="75" o:preferrelative="t"
 path="m@4@5l@4@11@9@11@9@5xe" filled="f" stroked="f">
 <v:stroke joinstyle="miter"/>
 <v:formulas>
  <v:f eqn="if lineDrawn pixelLineWidth 0"/>
  <v:f eqn="sum @0 1 0"/>
  <v:f eqn="sum 0 0 @1"/>
  <v:f eqn="prod @2 1 2"/>
  <v:f eqn="prod @3 21600 pixelWidth"/>
  <v:f eqn="prod @3 21600 pixelHeight"/>
  <v:f eqn="sum @0 0 1"/>
  <v:f eqn="prod @6 1 2"/>
  <v:f eqn="prod @7 21600 pixelWidth"/>
  <v:f eqn="sum @8 21600 0"/>
  <v:f eqn="prod @7 21600 pixelHeight"/>
  <v:f eqn="sum @10 21600 0"/>
 </v:formulas>
 <v:path o:extrusionok="f" gradientshapeok="t" o:connecttype="rect"/>
 <o:lock v:ext="edit" aspectratio="t"/>
</v:shapetype><v:shape id="Picture_x0020_1" o:spid="_x0000_i1025" type="#_x0000_t75"
 alt="" style='width:212.6pt;height:67.9pt'>
 <v:imagedata src="file:///C:/Users/Heinrich/AppData/Local/Temp/msohtmlclip1/01/clip_image001.jpg"
  o:href="cid:image002.jpg@01D7CC25.C8ECFE30"/>
</v:shape><![endif]-->
      &nbsp;<img width="283" height="91" src="AusBat_files/image-322269678091160930.jpg" v="#DEFAULT" shapes="Picture_x0020_1" style="height: .943in; width: 2.943in"/>&nbsp;&nbsp;e.g. [Austonomus australis]<o p="#DEFAULT"></o></font></span>
    </p>
    <p class="MsoPlainText">
      <span lang="EN-AU" style="color: #00B0F0"><font color="#00B0F0"><o p="#DEFAULT">
      &nbsp;</o></font></span>
    </p>
    <p class="MsoPlainText">
      <span lang="EN-AU" style="color: #00B0F0"><font color="#00B0F0">Then convert it to all lowercase, spaces replaced with underscores, and a prefix added to distinguish data types. For the details file it becomes &nbsp;&nbsp;[austonomus_australis_details.html]<o p="#DEFAULT"></o></font></span>
    </p>
    <p class="MsoPlainText">
      <span lang="EN-AU" style="color: #00B0F0"><font color="#00B0F0"><o p="#DEFAULT">
      &nbsp;</o></font></span>
    </p>
    <p class="MsoPlainText">
      <span lang="EN-AU" style="color: #00B0F0"><font color="#00B0F0">For species we have the following types<o p="#DEFAULT"></o></font></span>
    </p>
    <p class="MsoPlainText">
      <span lang="EN-AU" style="color: #00B0F0"><font color="#00B0F0">Datasets [_dataset.html] e.g. austonomus_australis_dataset.json<o p="#DEFAULT"></o></font></span>
    </p>
    <p class="MsoPlainText">
      <span lang="EN-AU" style="color: #00B0F0"><font color="#00B0F0">Details [_details.json] e.g. austonomus_australis_details.html<o p="#DEFAULT"></o></font></span>
    </p>
    <p class="MsoPlainText">
      <span lang="EN-AU" style="color: #00B0F0"><font color="#00B0F0">Hires images:<o p="#DEFAULT"></o></font></span>
    </p>
    <p class="MsoPlainText">
      <span lang="EN-AU" style="color: #00B0F0"><font color="#00B0F0">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Call images [_call_image] e.g. austonomus_australis_call_image.jpg<o p="#DEFAULT"></o></font></span>
    </p>
    <p class="MsoPlainText">
      <span lang="EN-AU" style="color: #00B0F0"><font color="#00B0F0">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Distribution maps [_dist] e.g. austonomus_australis_dist.jpg<o p="#DEFAULT"></o></font></span>
    </p>
    <p class="MsoPlainText">
      <span lang="EN-AU" style="color: #00B0F0"><font color="#00B0F0">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Face image [_head] e.g. austonomus_australis_head.jpg<o p="#DEFAULT"></o></font></span>
    </p>
    <p class="MsoPlainText">
      <span lang="EN-AU" style="color: #00B0F0"><font color="#00B0F0">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Other images (just add number after name e.g. austonomus_australis1.jpg<o p="#DEFAULT"></o>&nbsp;</font></span>
    </p>
    <p class="MsoPlainText">
      
    </p>
    <p class="MsoPlainText">
      
    </p>
  </body>
</html></richcontent>
</node>
</node>
<node TEXT="Process to convert RTF to HTML" POSITION="left" ID="ID_407010586" CREATED="1638424070503" MODIFIED="1638424101540">
<edge COLOR="#007c7c"/>
<richcontent TYPE="NOTE" CONTENT-TYPE="xml/">
<html>
  <head>
    
  </head>
  <body>
    <p class="MsoNormal">
      <span>Process to convert the RTF to Html is as follows:<o p="#DEFAULT"></o></span>
    </p>
    <p class="MsoNormal">
      <span>&nbsp;<o p="#DEFAULT"></o></span>
    </p>
    <ol start="1" type="1" style="margin-top: 0in">
      <li class="MsoListParagraph" style="margin-left: 0in">
        <span>Open the RTF &nbsp;file in WORD.<o p="#DEFAULT"></o></span>
      </li>
      <li class="MsoListParagraph" style="margin-left: 0in">
        <span>Select all text in the file using [Ctrl-A], copy it to clipboard [Ctrl-C] <o p="#DEFAULT"></o></span>
      </li>
      <li class="MsoListParagraph" style="margin-left: 0in">
        <span>Open up the HTML editor at &nbsp;[https://html-online.com/editor/], then select [File-&gt;New document]<o p="#DEFAULT"></o></span>
      </li>
      <li class="MsoListParagraph" style="margin-left: 0in">
        <span>Paste the text into the left-hand side of the editor [Ctrl-V]<o p="#DEFAULT"></o></span>
      </li>
      <li class="MsoListParagraph" style="margin-left: 0in">
        <span>Copy the generated text (HTML) in the right-hand side of the editor to the clipboard &nbsp;by clicking on the right-hand box, then [Ctrl-A, Ctrl-C]<o p="#DEFAULT"></o></span>
      </li>
      <li class="MsoListParagraph" style="margin-left: 0in">
        <span>Paste the clipboard [Ctrl-V] to a &nbsp;new text file which gets named [genusname_speciesname_details.html]. <o p="#DEFAULT"></o></span>
      </li>
    </ol>
  </body>
</html></richcontent>
</node>
</node>
</map>
