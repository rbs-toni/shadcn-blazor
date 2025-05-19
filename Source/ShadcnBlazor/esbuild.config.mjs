import { readdir } from 'fs/promises'
import * as esbuild from 'esbuild'
import path from 'path'

const ROOT_DIR = './wwwroot'
const SCRIPTS_DIR = './Scripts'
const OUTPUT_DIR = './wwwroot/modules'

const build = input => {
    const isShadcnBlazor = input === 'ShadcnBlazor.js'
    const outDir = isShadcnBlazor ? ROOT_DIR : OUTPUT_DIR
    const outputFilename = isShadcnBlazor
        ? 'ShadcnBlazor.lib.module.js'
        : input.replace(/\.js$/, '.min.js')

    return esbuild.build({
        entryPoints: [path.join(SCRIPTS_DIR, input)],
        bundle: true,
        minify: !isShadcnBlazor, // Only minify non-Shadcn files
        target: 'es2022',
        format: 'esm',
        outfile: path.join(outDir, outputFilename),
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
        console.log('All scripts built successfully:')
        console.log('- ShadcnBlazor.js → wwwroot/ShadcnBlazor.lib.module.js')
        console.log(`- Other files → ${OUTPUT_DIR}/[filename].min.js`)
    } catch (err) {
        console.error('Build failed:', err)
        process.exit(1)
    }
}

buildAll()