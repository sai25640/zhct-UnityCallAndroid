package com.example.videolibrary;


import android.content.Context;
import android.database.Cursor;
import android.os.Bundle;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;
import android.content.Intent;
import java.io.File;
import android.net.Uri;
import android.provider.MediaStore;
import android.support.v4.content.FileProvider;
import android.util.Log;
import android.media.MediaMetadataRetriever;

import static android.content.ContentValues.TAG;

public class MainActivity extends UnityPlayerActivity {

    private static final String FILE_PATH = "/sdcard/com.hty.cyxq/sysvideocamera.mp4";//

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    public void ShowSysVideoCamera() {
        Intent intent = new Intent();
        intent.setAction("android.media.action.VIDEO_CAPTURE");
        intent.addCategory("android.intent.category.DEFAULT");
//        File file = new File(FILE_PATH);
//        if(file.exists()){
//            file.delete();
//        }
//       Uri uri = Uri.fromFile(file);

        //intent.setFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
        //Uri uri = FileProvider.getUriForFile(this,this.getPackageName()+".fileProvider",file);
       //intent.putExtra(MediaStore.EXTRA_OUTPUT, uri);


        startActivityForResult(intent, 0);
    }


    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        Log.i(TAG, "拍摄完成，resultCode="+requestCode);

        String realPath = getRealPathFromUri(this,data.getData());

        UnityPlayer.UnitySendMessage("Main Camera","ShowUpLoadView",realPath);

    }

    public static String getRealPathFromUri(Context context, Uri contentUri) {
        Cursor cursor = null;
        try {
            String[] proj = { MediaStore.Images.Media.DATA };
            cursor = context.getContentResolver().query(contentUri, proj, null, null, null);
            int column_index = cursor.getColumnIndexOrThrow(MediaStore.Images.Media.DATA);
            cursor.moveToFirst();
            return cursor.getString(column_index);
        } finally {
            if (cursor != null) {
                cursor.close();
            }
        }
    }

}
