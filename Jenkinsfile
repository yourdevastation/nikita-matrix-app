pipeline {
    agent none
    
    options {
        skipDefaultCheckout(true)
    }

    parameters {
        string(name: 'VERSION', defaultValue: '1.1.0', description: 'Version to deploy')
    }

    environment {
        DOCKER_REPO = 'gripsss'
        DOCKER_CREDENTIALS_ID = 'dockerhub-credentials'
        IMAGE_NAME = 'matrix-app-mono'
        IMAGE_TAG = "${params.VERSION}-${BUILD_NUMBER}"
    }
    stages {
        stage('Prepare') {
            agent { label 'build-test-agent' }
            steps {
                checkout scm
                stash includes: '**', name: 'source-code'
            }
        }

        stage('Build Test Image') {
            agent { label 'build-test-agent' }
            steps {
                unstash 'source-code'
                sh 'docker build -t matrix-app-test:${BUILD_NUMBER} . -f Dockerfile.test'
            }
        }

        stage('Test') {
            agent { label 'build-test-agent' }
            when {
                expression { currentBuild.resultIsBetterOrEqualTo('SUCCESS') }
            }
            steps {
                sh 'docker run --rm \
                    -v \$(pwd)/testresults:/src/TestResults \
                    matrix-app-test:${BUILD_NUMBER}'
            }
            post {
                always {
                    junit 'testresults/*.trx'
                }
            }
        }
        
        stage('Build Production Image') {
            agent { label 'build-test-agent' }
            when {
                expression { currentBuild.resultIsBetterOrEqualTo('SUCCESS') }
            }
            steps {
                unstash 'source-code'
                sh '''
                docker build -t ${DOCKER_REPO}/${IMAGE_NAME}:${IMAGE_TAG} .
                docker tag ${DOCKER_REPO}/${IMAGE_NAME}:${IMAGE_TAG} ${DOCKER_REPO}/${IMAGE_NAME}:latest
                '''
            }
        }

        stage('Push to Registry') {
            agent { label 'build-test-agent' }
            when {
                expression { currentBuild.resultIsBetterOrEqualTo('SUCCESS') }
            }
            steps {
                withCredentials([usernamePassword(credentialsId: "${DOCKER_CREDENTIALS_ID}",
                                                usernameVariable: 'DOCKERHUB_USER',
                                                passwordVariable: 'DOCKERHUB_PASS')]) {
                                                
                    sh '''
                    echo $DOCKERHUB_PASS | docker login -u $DOCKERHUB_USER --password-stdin
                    docker push ${DOCKER_REPO}/${IMAGE_NAME}:${IMAGE_TAG}
                    docker push ${DOCKER_REPO}/${IMAGE_NAME}:latest
                    docker logout
                    '''
                }
            }
        }
        
        stage('Deploy') {
            agent { label 'deploy-agent' }
            when {
                expression { currentBuild.resultIsBetterOrEqualTo('SUCCESS') }
            }
            steps {
                sh '''
                docker pull ${DOCKER_REPO}/${IMAGE_NAME}:${IMAGE_TAG}
                docker stop ${IMAGE_NAME} || true
                docker rm ${IMAGE_NAME} || true
                docker run -d -p 5000:8080 --name ${IMAGE_NAME} ${DOCKER_REPO}/${IMAGE_NAME}:${IMAGE_TAG}
                '''
            }
        }
    }
    
    post {
        success {
            node('build-test-agent') {
                echo 'Build and deployment completed successfully!'
            }
        }
        failure {
            node('build-test-agent') {
                echo 'Build or tests failed!'
            }
        }
        always {
            node('build-test-agent') {
                sh 'docker system prune -f'
                cleanWs() 
            }
        }
    }
    
}