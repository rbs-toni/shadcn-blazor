import { readdir } from 'fs/promises'
import * as esbuild from 'esbuild'
import path from 'path'

const SCRIPTS_DIR = './Scripts'
const OUTPUT_DIR = './wwwroot/modules'

const build = input => {
    return esbuild.build({
        entryPoints: [path.join(SCRIPTS_DIR, input)],
        bundle: true,
        minify: true,
        target: 'es2022',
        format: 'esm',
        outfile: path.join(OUTPUT_DIR, input.replace(/\.js$/, '.min.js')),
    })
}

async function buildAll() {
    try {
        const files = await readdir(SCRIPTS_DIR)
        const scripts = files.filter(f => f.endsWith('.js'))

        if (scripts.length === 0) {
            console.warn('No scripts found to build.')
            return
        }

        await Promise.all(scripts.map(build))
        console.log('All scripts built successfully.')
    } catch (err) {
        console.error('Build failed:', err)
        process.exit(1)
    }
}

buildAll()
