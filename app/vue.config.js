/*=========================================================================================
  File Name: vue.config.js
  Description: configuration file of vue
  ----------------------------------------------------------------------------------------
  Item Name: Vuexy - Vuejs, HTML & Laravel Admin Dashboard Template
  Author: Pixinvent
  Author URL: http://www.themeforest.net/user/pixinvent
==========================================================================================*/

//const fs = require('fs')

module.exports = {
  publicPath: '/',
  transpileDependencies: [
    'vue-echarts',
    'resize-detector'
  ],
  configureWebpack: {
    optimization: {
      splitChunks: {
        chunks: 'all'
      }
    },
    resolve: {
      alias: {
        'vue$': 'vue/dist/vue.esm.js'
      }
    } ,
  
  },
  //runtimeCompiler: true,
  /*devServer: {
    https: {
      key: fs.readFileSync('./certs/example.com+5-key.pem'),
      cert: fs.readFileSync('./certs/example.com+5.pem'),
    },
    public: 'https://localhost:8080/'
  } */  

}

