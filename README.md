# SeeCam
Simple camera preview application for Windows.

Windows PCで、カメラ（PC搭載WebカメラやUSB外付けなど）の映像をプレビューするだけのアプリケーションです。ビデオ会議で画面共有したいが自分の姿も入れたい、OBSやmmhmmは合わない、といった場面などで便利じゃないかと思います。

- 【重要】Windows 10専用です。
- 操作は「起動する」「カメラ切替やタイトルバー消去はウインドウ内を右クリック」だけです。
- カメラを切り替えられます。
- タイトルバーなどを全て隠したり、再度表示したりできます。なおタイトルバーなどを隠した状態ではウインドウの移動や拡大縮小ができないので、必要な場合はタイトルバーを再度表示してください。
- カメラ映像はウインドウ内にぴったり収まるように（元のアスペクト比のままで）拡大縮小します。カメラ映像とウインドウのアスペクト比が違う場合、余った領域は黒くなります。

![SeeCamスクリーンショット](https://github.com/sksthrs/SeeCam/wiki/seecam_ss.png)

## 中身について

- 基本的にはAForge.NET ( http://www.aforgenet.com/ ) を使っているだけです。なおSeeCamはMITライセンスですが、AForge.NETはLGPLライセンスです。
- tocsworldさんの記事 ( https://tocsworld.wordpress.com/2014/02/25/c%e3%81%ab%e3%82%88%e3%82%8busb%e3%82%ab%e3%83%a1%e3%83%a9%e6%93%8d%e4%bd%9c/ ) を参考にしました。
