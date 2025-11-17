import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
    plugins: [react()],
    server: {
        port: 62502,
        proxy: {
            '/api': {
                target: 'https://localhost:7242', // твой .NET API
                changeOrigin: true,
                secure: false, // разрешить самоподписанный сертификат
            },
        },
    },
})