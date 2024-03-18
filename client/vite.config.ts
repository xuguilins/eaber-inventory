import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, 'src')
    }
  },
  server: {
    host: '0.0.0.0',
    proxy:{
      '/api':{
        changeOrigin:true,
        target: 'http://localhost:5269',
        rewrite: (path) => path.replace(/^\/api/, '')
      }
    }
  }
})
